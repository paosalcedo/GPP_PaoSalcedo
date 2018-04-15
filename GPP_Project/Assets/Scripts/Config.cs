using System;
using UnityEngine;

[CreateAssetMenu (menuName = "Difficulty")]
[Serializable]

public class Difficulty : ScriptableObject
{
    [SerializeField] private int _maxNumEnemiesInWave;

    public int MaxNumEnemiesInWave
    {
        get { return _maxNumEnemiesInWave; }
    }
    
    [SerializeField] private int _minNumEnemiesInWave;

    public int MinNumEnemiesInWave
    {
        get { return _minNumEnemiesInWave; }
    }

    [SerializeField] private string _myName;

    public string MyName
    {
        get { return _myName; }
    }
}
