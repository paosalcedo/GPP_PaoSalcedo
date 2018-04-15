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
		Services.Scenes.PushScene<GameScene>(new TransitionData(_easy));
	}

	public void SelectNormal()
	{
		Services.Scenes.PushScene<GameScene>(new TransitionData(_normal));
	}

	public void SelectHard()
	{		
		Services.Scenes.PushScene<GameScene>(new TransitionData(_hard));
	}
}
