using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : Scene<TransitionData> {

	private Difficulty _difficulty;
	private int _score;
	[SerializeField]private int _startNumEnemies;
	[SerializeField]private int _maxNumEnemiesInWave;
	[SerializeField] private int enemiesSpawned;
	[SerializeField] private float enemyEmissionTime;

	protected override void OnEnter(TransitionData data){
		_difficulty = data.difficulty;
 		Services.EnemyManager.maxNumEnemiesInWave = _difficulty.MaxNumEnemiesInWave;
		Services.EnemyManager.minNumEnemiesInWave = _difficulty.MinNumEnemiesInWave;
	}
	
	void Start () {
		Services.EnemyManager.Start();
	}
	
	// Update is called once per frame
	void Update () {
		Services.EnemyManager.Update();
		enemiesSpawned = Services.EnemyManager.enemiesSpawned;
		enemyEmissionTime = Services.EnemyManager.enemyEmissionTime;
	}

}
