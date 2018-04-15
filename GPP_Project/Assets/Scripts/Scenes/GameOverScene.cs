using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScene : Scene<TransitionData>
{

	[SerializeField] private Text _scoreText;
	// Use this for initialization
	protected override void OnEnter(TransitionData data)
	{
		_scoreText.text = "You destroyed " + data.score + " " + data.difficultyName + " enemies!";
	}

	public void Restart()
	{
		
	}
}
