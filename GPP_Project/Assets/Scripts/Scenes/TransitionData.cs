using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionData
{

    public readonly Difficulty difficulty;
    public readonly int score;
    public readonly string difficultyName;

    public TransitionData(Difficulty difficulty = null, string name = "", int score = 0)
    {
        this.difficulty = difficulty;
        this.score = score;
        this.difficultyName = name;
    }

}
