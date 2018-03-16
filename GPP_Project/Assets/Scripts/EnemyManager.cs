using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Rewired.ComponentControls.Effects;
using UnityEngine;

public class EnemyManager {
	public static EnemyManager enemyManager { get; set; }
	public List<GameObject> enemiesInWave = new List<GameObject>();
	public int wavesDefeated = 0;
	public int waveNum = 0;
	public int enemiesDead;
	public int enemiesToSpawn;
	private bool bossAdded = false;
	
	public void Start ()
	{
		waveNum = 3;
		enemyEmissionTime = 5;
		enemiesToSpawn = 1;
		EventManager.Instance.Register<EnemyDeath>(CountEnemyDeath);
//		SpawnSpecificEnemy(3, Player.instance.transform.position + Vector3.forward * 1000);
	}
	
	// Update is called once per frame
	public void Update () {
//		if(enemiesInWave.Count==0){
//			wavesDefeated++;
//			waveNum++;
//		}
 		
		EmitWave(waveNum, enemiesToSpawn);

		if (enemiesDead == enemiesInWave.Count && enemiesDead != 0)
		{
			enemiesInWave.Clear();
			enemiesSpawned = 0;
			waveNum += 1;
			if (waveNum != 3)
			{
				enemiesToSpawn = Random.Range(3, 8);			
			} else if (waveNum == 3)
			{
				enemiesToSpawn = 1;
			}

			Debug.Log("Enemiees in next wave: " + enemiesToSpawn);
			enemiesDead = 0;
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

	public void SpawnSpecificEnemy(int _enemyId, Vector3 _pos)
	{
		enemiesSpawned++;
		
		switch (_enemyId){
			case 0: //Homing Enemy
				GameObject homingEnemy = new GameObject("HomingEnemy");
				homingEnemy.AddComponent<Homing>();
// 				homingEnemy.transform.position = Vector3.forward * Random.Range(50,100) + new Vector3 (Random.Range(-9,9), 0, Random.Range(50,100));				
				homingEnemy.transform.position = _pos + Vector3.forward * 100;
				enemiesInWave.Add(homingEnemy); 
 			break;
			case 1: //sniper enemy
 				GameObject sniperEnemy = new GameObject ("SniperEnemy");
				sniperEnemy.AddComponent<Sniper>();
 				sniperEnemy.transform.position = Vector3.forward * Random.Range(50,100) + new Vector3 (Random.Range(-9,9), 0, Random.Range(50,100));				
				enemiesInWave.Add(sniperEnemy);
 			break;
			case 2: //speedy enemy
				GameObject speedyEnemy = new GameObject ("SpeedyEnemy");
				speedyEnemy.AddComponent<Speedy>();
				speedyEnemy.transform.position = Vector3.forward * Random.Range(50,100) + new Vector3 (Random.Range(-9,9), 0, Random.Range(50,100));				
				enemiesInWave.Add(speedyEnemy);
//				GameObject homingEnemy0 = new GameObject("HomingEnemy");
//				homingEnemy0.AddComponent<Homing>();
//				homingEnemy0.transform.position = Vector3.forward * Random.Range(50,100) + new Vector3 (Random.Range(-9,9), 0, Random.Range(50,100));				
//				enemiesInWave.Add(homingEnemy0);
			break;
			case 3: //boss
				GameObject boss = new GameObject("Boss");
 				boss.AddComponent<Boss>();
//				boss.GetComponent<Boss>().MyBehaviorPattern();
				enemiesInWave.Add(boss);
			break;
			default:
			break;
		}
	}

	public int enemiesSpawned = 0;
	
	public float enemyEmissionTime;
	
	public void EmitWave(int _waveNum, int _numEnemies){
		
		switch (_waveNum){
			case 0:
				Debug.Log("This is wave " + waveNum);
				if (enemiesSpawned < _numEnemies)
				{
					enemyEmissionTime += Time.deltaTime;
					if (enemyEmissionTime >= 5)
					{
 						SpawnSpecificEnemy(0, Vector3.zero);
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
						SpawnSpecificEnemy(1, Vector3.zero);
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
						SpawnSpecificEnemy(2, Vector3.zero);
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
						SpawnSpecificEnemy(3, Vector3.zero);
						enemyEmissionTime = 0;
					}
				}
				break;

			default:
			break;
		}
	}
	
	public void PopulateWaveFromPosition(int _numEnemies, Vector3 _pos){
		for (int i = 0; i<_numEnemies; i++){
//			GameObject speedyEnemy = new GameObject ("SpeedyEnemy");
//			speedyEnemy.AddComponent<Speedy>();
//			speedyEnemy.transform.position = _pos;				
//			enemiesInWave.Add(speedyEnemy);
			GameObject homingEnemy = new GameObject("HomingEnemy");
			homingEnemy.AddComponent<Homing>();
			homingEnemy.transform.position = _pos;		
			enemiesInWave.Add(homingEnemy);
		}
	}

	void WaveTimer(){
	}
	
	public void CountEnemyDeath(GameEvent e)
	{
		EnemyDeath enemyDeath = e as EnemyDeath;
		enemiesDead++;
	}
}
