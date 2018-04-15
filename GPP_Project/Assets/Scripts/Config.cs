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
}
