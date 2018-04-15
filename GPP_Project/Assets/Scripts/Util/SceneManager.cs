using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class SceneManager<TTransitionData> {

    internal GameObject SceneRoot { get; private set; }
    
    private readonly Dictionary<Type, GameObject> _scenes = new Dictionary<Type, GameObject>();

    public SceneManager(GameObject root, IEnumerable<GameObject> scenePrefabs)
    {
        SceneRoot = root;

        foreach (var prefab in scenePrefabs)
        {
            var scene = prefab.GetComponent<Scene<TTransitionData>>();
            
        }
    }

}
