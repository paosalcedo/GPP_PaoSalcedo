using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene<TTransitionData> : MonoBehaviour {

    public GameObject Root
    {
        get { return gameObject; }
    }

    internal void _OnEnter(TTransitionData data)
    {
        Root.SetActive(true);
        OnEnter(data);
    }

    internal void _OnExit()
    {
        Root.SetActive(false);
        OnExit();
    }

    protected virtual void OnEnter(TTransitionData data)
    {
        
    }

    protected virtual void OnExit()
    {
        
    }
}
