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
		Cursor.lockState = CursorLockMode.None;
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.T))
		{
			ReturnToTitleScreen();
		}
	}

	public void ReturnToTitleScreen()
	{
		Debug.Log("Returning to title!");
		Services.Scenes.Swap<TitleScene>();
	}
}
