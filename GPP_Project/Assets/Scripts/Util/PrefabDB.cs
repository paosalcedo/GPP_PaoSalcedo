using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Prefab DB")]
public class PrefabDB : ScriptableObject
{
    [SerializeField] private GameObject[] _levels;

    public GameObject[] Levels
    {
        get { return _levels; }
    }
}
