﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Rewired.ComponentControls.Effects;
using UnityEngine;

public class EnemyManager {
	public static EnemyManager enemyManager { get; set; }
	public List<GameObject> enemiesInWave = new List<GameObject>();
	public List<GameObject> bossMinionWave = new List<GameObject>();
 	public int wavesDefeated = 0;
	public int waveNum = 0;
	public int enemiesDead;
	public int maxNumEnemiesInWave;
	public int minNumEnemiesInWave;
	private int enemiesToSpawn;
 	private int bossMinionsKilled;
	private int bossWaveSize = 5;
	private bool bossAdded = false;
	private GameObject _boss;
	[HideInInspector]public int score;
	
	public void Start ()
	{
		waveNum = 0;
		enemiesToSpawn = Random.Range(minNumEnemiesInWave, maxNumEnemiesInWave);			
		enemyEmissionTime = 5;
 		EventManager.Instance.Register<EnemyDeath>(CountEnemyDeath);
  	}
	
	// Update is called once per frame
	public void Update () {
 		EmitWave(waveNum, enemiesToSpawn);

		if (enemiesDead == enemiesInWave.Count && enemiesDead != 0)
		{
			enemiesInWave.Clear();
			enemiesSpawned = 0;
			waveNum += 1;
			if (waveNum != 3)
			{
				enemiesToSpawn = Random.Range(minNumEnemiesInWave, maxNumEnemiesInWave);			
			} else if (waveNum == 3)
			{
				enemiesToSpawn = 1;
			}

			enemiesDead = 0;
		}

		if (bossMinionsKilled == bossMinionWave.Count && bossMinionsKilled != 0)
		{
			bossMinionWave.Clear();
			bossMinionsKilled = 0;
		}
//		if(enemiesInWave.Count > 0){
//			foreach (var enemy in enemiesInWave){
//				if (enemy != null)
//				{
//					SubclassSandbox.Enemy _enemy = enemy.GetComponent<SubclassSandbox.Enemy>();
//					if(_enemy.health <= 0){
//						enemiesInWave.Remove(_enemy.gameObject);
//						_enemy.DestroyMe();
//					}
//				}
//			}
//		}

//		if (wavesDefeated == 3 && !bossAdded)
//		{
//			SpawnSpecificEnemy(3, Player.instance.transform.position + Vector3.forward * 400f);
//			bossAdded = true;
//		}
	}

//	public void SpawnSpecificEnemy(int _enemyId, Vector3 _pos)
//	{
//		enemiesSpawned++;
//		
//		switch (_enemyId){
//			case 0: //Homing Enemy
//				GameObject homingEnemy = new GameObject("HomingEnemy");
//				homingEnemy.AddComponent<Homing>();
// 				homingEnemy.transform.position = _pos;
//				enemiesInWave.Add(homingEnemy); 
// 			break;
//			case 1: //sniper enemy
// 				GameObject sniperEnemy = new GameObject ("SniperEnemy");
//				sniperEnemy.AddComponent<Sniper>();
// 				sniperEnemy.transform.position = _pos;				
//				enemiesInWave.Add(sniperEnemy);
// 			break;
//			case 2: //speedy enemy
//				GameObject speedyEnemy = new GameObject ("SpeedyEnemy");
//				speedyEnemy.AddComponent<Speedy>();
//				speedyEnemy.transform.position = _pos;				
//				enemiesInWave.Add(speedyEnemy);
//			break;
//			case 3: //boss
//				GameObject boss = new GameObject("Boss");
// 				boss.AddComponent<Boss>();
// 				enemiesInWave.Add(boss);
//			break;
//			default:
//			break;
//		}
//	}

	public GameObject SpawnSpecificEnemy(int _enemyId, Vector3 _pos)
	{
		enemiesSpawned++;
		switch (_enemyId){
			case 0: //Homing Enemy
				GameObject homingEnemy = new GameObject("HomingEnemy");
				homingEnemy.AddComponent<Homing>();
 				homingEnemy.transform.position = _pos;
				homingEnemy.transform.SetParent(MonoBehaviour.FindObjectOfType<GameScene>().transform);
				return homingEnemy;
			case 1: //sniper enemy
 				GameObject sniperEnemy = new GameObject ("SniperEnemy");
				sniperEnemy.AddComponent<Sniper>();
 				sniperEnemy.transform.position = _pos;
				sniperEnemy.transform.SetParent(MonoBehaviour.FindObjectOfType<GameScene>().transform);
				return sniperEnemy;
 			case 2: //speedy enemy
				GameObject speedyEnemy = new GameObject ("SpeedyEnemy");
				speedyEnemy.AddComponent<Speedy>();
				speedyEnemy.transform.position = _pos;
				speedyEnemy.transform.SetParent(MonoBehaviour.FindObjectOfType<GameScene>().transform);
				return speedyEnemy;
 			case 3: //boss
				GameObject boss = new GameObject("Boss");
 				boss.AddComponent<Boss>();
				boss.transform.SetParent(MonoBehaviour.FindObjectOfType<GameScene>().transform);
				_boss = boss;
				return boss;
 			default:
				return null;
 		}	
	}

	public int enemiesSpawned = 0;
	
	public float enemyEmissionTime;
	
	public void EmitWave(int _waveNum, int _numEnemies){
		Debug.Log("Wave emitting!");
		switch (_waveNum){
			case 0:
				Debug.Log("This is wave " + waveNum);
				if (enemiesSpawned < _numEnemies)
				{
					enemyEmissionTime += Time.deltaTime;
					if (enemyEmissionTime >= 5)
					{
 						enemiesInWave.Add(SpawnSpecificEnemy(0, Player.instance.transform.position + Vector3.forward * 100f));
						enemyEmissionTime = 0;
					}
				}
			break;
			case 1: 
				Debug.Log("This is wave " + waveNum);
				if (enemiesSpawned < _numEnemies)
				{
					enemyEmissionTime += Time.deltaTime;
					if (enemyEmissionTime >= 1)
					{
//						SpawnSpecificEnemy(1, Vector3.forward * 100f);
						enemiesInWave.Add(SpawnSpecificEnemy(1, Player.instance.transform.position + Vector3.forward * 100f));
						enemyEmissionTime = 0;
					}
				}
			break;
			case 2: 
				Debug.Log("This is wave " + waveNum);
				if (enemiesSpawned < _numEnemies)
				{
					enemyEmissionTime += Time.deltaTime;
					if (enemyEmissionTime >= 1)
					{
//						SpawnSpecificEnemy(2, Vector3.forward * 100f);
						enemiesInWave.Add(SpawnSpecificEnemy(2, Player.instance.transform.position + Vector3.forward * 100f));
						enemyEmissionTime = 0;
					}
				}
			break;
			case 3: 
				Debug.Log("This is wave " + waveNum);
				if (enemiesSpawned < _numEnemies)
				{
					enemyEmissionTime += Time.deltaTime;
					if (enemyEmissionTime >= 1)
					{
						enemiesInWave.Add(SpawnSpecificEnemy(3, Vector3.zero));
						enemyEmissionTime = 0;
					}
				}
				break;

			default:
			break;
		}
	}
	
	public void SpawnBossMinionWave(int _numEnemies, Vector3 _pos){
		for (int i = 0; i<_numEnemies; i++){
//			GameObject speedyEnemy = new GameObject ("SpeedyEnemy");
//			speedyEnemy.AddComponent<Speedy>();
//			speedyEnemy.transform.position = _pos;				
//			enemiesInWave.Add(speedyEnemy);
			if (bossMinionWave.Count < bossWaveSize)
			{
				bossMinionWave.Add(SpawnSpecificEnemy(Random.Range(0,2), _pos));				
			}
		}
	}

	void WaveTimer(){
	}
	
	public void CountEnemyDeath(GameEvent e)
	{
		EnemyDeath enemyDeath = e as EnemyDeath;
		if (waveNum != 3)
		{
			enemiesDead++;
			score++;
		}
		else
		{
			bossMinionsKilled++;
			score++;
		}


	}
}
