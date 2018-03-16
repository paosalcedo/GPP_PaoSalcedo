using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{

	[SerializeField] private int enemiesSpawned;
	[SerializeField] private float enemyEmissionTime;
	void Awake(){
		EnemyManager.enemyManager = new EnemyManager();
	}
	// Use this for initialization
	void Start () {
		EnemyManager.enemyManager.Start();
	}
	
	// Update is called once per frame
	void Update () {
		EnemyManager.enemyManager.Update();
		enemiesSpawned = EnemyManager.enemyManager.enemiesSpawned;
		enemyEmissionTime = EnemyManager.enemyManager.enemyEmissionTime;
		
		Debug.Log("Waves defeated = " + EnemyManager.enemyManager.wavesDefeated);
	
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Debug.Log(EnemyManager.enemyManager.enemiesInWave.Count);
		}
	}
}
