using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;

public class SceneManager<TTransitionData> {
    
    internal GameObject SceneRoot { get; private set; }
    
    private readonly Dictionary<Type, GameObject> _scenes = new Dictionary<Type, GameObject>();

    public SceneManager(GameObject root, IEnumerable<GameObject> scenePrefabs)
    {
        SceneRoot = root;

        foreach (var prefab in scenePrefabs)
        {
            var scene = prefab.GetComponent<Scene<TTransitionData>>();   
            
            Assert.IsNotNull(scene, "Couldn't find scene script in prefab" );
            
            _scenes.Add(scene.GetType(), prefab);
        }
    }

    private readonly Stack<Scene<TTransitionData>> _sceneStack = new Stack<Scene<TTransitionData>>();

    public Scene<TTransitionData> CurrentScene
    {
        get { return _sceneStack.Count != 0 ? _sceneStack.Peek() : null; }
    }

    public void PopScene(TTransitionData data = default (TTransitionData))
    {
        Scene<TTransitionData> previousScene = null;
        Scene<TTransitionData> nextScene = null;

        if (_sceneStack.Count != 0)
        {
            previousScene = _sceneStack.Peek();
            _sceneStack.Pop();
        }

        if (_sceneStack.Count != 0)
        {
            //since last one was popped, 
            //next scene can be peeked
            nextScene = _sceneStack.Peek();
        }

        if (nextScene != null)
        {
            nextScene._OnEnter(data);
        }

        if (previousScene != null)
        {
            Object.Destroy(previousScene.Root);
            previousScene._OnExit();
        }
    }
    
    //creates the scene
    public void PushScene<T>(TTransitionData data = default (TTransitionData)) where T : Scene<TTransitionData>
    {
        var previousScene = CurrentScene;
        var nextScene = GetScene<T>();
        
        _sceneStack.Push(nextScene);
        nextScene._OnEnter(data);

        if (previousScene != null)
        {
            previousScene._OnExit();
            previousScene.Root.SetActive(false);
        }
    }

    public void Swap<T>(TTransitionData data = default(TTransitionData)) where T : Scene<TTransitionData>
    {
        Scene<TTransitionData> previousScene = null;
        if (_sceneStack.Count != 0)
        {
            previousScene = _sceneStack.Peek();
            _sceneStack.Pop();
        }

        var nextScene = GetScene<T>();
        _sceneStack.Push(nextScene);
        nextScene._OnEnter(data);

        if (previousScene != null)
        {
            previousScene._OnExit();
            Object.Destroy(previousScene.Root);
        }
    }
    
    public void Switch<T>(TTransitionData data = default(TTransitionData)) where T : Scene<TTransitionData>
    {
        var previousScene = CurrentScene;//assign GameScene to prevScene
        var nextScene = GetScene<T>(); //assign PauseScene to nextScene
        
        nextScene._OnEnter(data); //call OnEnter on PauseScene

        if (previousScene != null) //is true for GameScene
        {
            previousScene.Root.SetActive(false); //just sets it to false.
        }
    }


    private T GetScene<T>() where T : Scene<TTransitionData>
    {
        GameObject prefab;
        _scenes.TryGetValue(typeof(T), out prefab);
        Assert.IsNotNull(prefab, "Could not find scene prefab for scen type: " + typeof(T).Name);

        var sceneObject = Object.Instantiate(prefab);
        sceneObject.name = typeof(T).Name;
        sceneObject.transform.SetParent(SceneRoot.transform, false);
        return sceneObject.GetComponent <T>();
    }
}
