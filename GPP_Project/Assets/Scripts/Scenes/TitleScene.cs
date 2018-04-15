using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScene : Scene<TransitionData>
{

	[SerializeField] private Difficulty _easy;

	[SerializeField] private Difficulty _normal;

	[SerializeField] private Difficulty _hard;
	// Use this for initialization
	
	public void SelectEasy()
	{	
		Debug.Log("Selecdted easy");
		Services.Scenes.PushScene<GameScene>(new TransitionData(_easy, _easy.MyName));
	}

	public void SelectNormal()
	{
		Debug.Log("Selecdted normal");

		Services.Scenes.PushScene<GameScene>(new TransitionData(_normal, _normal.MyName));
	}

	public void SelectHard()
	{		
		Debug.Log("Selecdted hard");
		Services.Scenes.PushScene<GameScene>(new TransitionData(_hard, _hard.MyName));
	}
}
