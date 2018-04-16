using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class GameScene : Scene<TransitionData> {

	private Difficulty _difficulty;
	private int _score;
	[SerializeField]private int _minNumEnemiesInWave;
	[SerializeField]private int _maxNumEnemiesInWave;
	[SerializeField] private int enemiesSpawned;
	[SerializeField] private float enemyEmissionTime;

	protected override void OnEnter(TransitionData data){
		_difficulty = data.difficulty;
 		Services.EnemyManager.maxNumEnemiesInWave = _difficulty.MaxNumEnemiesInWave;
		Services.EnemyManager.minNumEnemiesInWave = _difficulty.MinNumEnemiesInWave;
		_minNumEnemiesInWave = Services.EnemyManager.minNumEnemiesInWave;
		_maxNumEnemiesInWave = Services.EnemyManager.maxNumEnemiesInWave;
		InstantiatePlayer();		
	}
	
	void Start () {
		Services.EnemyManager.Start();

		Debug.Log("Gamescene start!");
	}

	void InstantiatePlayer()
	{
		GameObject player = Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
		player.transform.position = Vector3.zero;
		Debug.Log("Player instantiated!");
	}

	// Update is called once per frame
	void Update () {
		Services.EnemyManager.Update();
		enemiesSpawned = Services.EnemyManager.enemiesSpawned;
		enemyEmissionTime = Services.EnemyManager.enemyEmissionTime;

		if (Input.GetKeyDown(KeyCode.P))
		{
			Services.Scenes.PushScene<GameOverScene>(new TransitionData(_difficulty, _difficulty.MyName, Services.EnemyManager.score));
		}
	}

}
