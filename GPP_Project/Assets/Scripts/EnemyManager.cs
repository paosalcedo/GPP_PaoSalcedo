using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Rewired.ComponentControls.Effects;
using UnityEngine;

public class EnemyManager {
	public static EnemyManager enemyManager { get; set; }
	public List<GameObject> enemiesInWave = new List<GameObject>();
	public int wavesDefeated = 0;
	private bool bossAdded = false;
	public void Start () {
//		PopulateWave(0, 5);
		SpawnSpecificEnemy(3, Player.instance.transform.position + Vector3.forward * 1000);
	}
	
	// Update is called once per frame
	public void Update () {
		if(enemiesInWave.Count==0){
			wavesDefeated++;
			PopulateWave(3, 1);
 		}
 		 
		if(enemiesInWave.Count > 0){
			foreach (var enemy in enemiesInWave){
				if (enemy != null)
				{
					SubclassSandbox.Enemy _enemy = enemy.GetComponent<SubclassSandbox.Enemy>();
					if(_enemy.health <= 0){
						enemiesInWave.Remove(_enemy.gameObject);
						_enemy.DestroyMe();
					}
				}
			}
		}

//		if (wavesDefeated == 3 && !bossAdded)
//		{
//			SpawnSpecificEnemy(3, Player.instance.transform.position + Vector3.forward * 400f);
//			bossAdded = true;
//		}
	}

	public void SpawnSpecificEnemy(int _enemyId, Vector3 _pos)
	{
		switch (_enemyId){
			case 0:
				GameObject homingEnemy = new GameObject("HomingEnemy");
				homingEnemy.AddComponent<Homing>();
				homingEnemy.transform.position = Vector3.forward * Random.Range(50,100) + new Vector3 (Random.Range(-9,9), 0, Random.Range(50,100));				
				enemiesInWave.Add(homingEnemy); 
 			break;
			case 1: 
 				GameObject sniperEnemy = new GameObject ("SniperEnemy");
				sniperEnemy.AddComponent<Sniper>();
				sniperEnemy.transform.position = Vector3.forward * Random.Range(50,100) + new Vector3 (Random.Range(-9,9), 0, Random.Range(50,100));				
				enemiesInWave.Add(sniperEnemy);
 			break;
			case 2: //speedy enemy
//				GameObject speedyEnemy = new GameObject ("SpeedyEnemy");
//				speedyEnemy.AddComponent<Speedy>();
//				speedyEnemy.transform.position = Vector3.forward * Random.Range(50,100) + new Vector3 (Random.Range(-9,9), 0, Random.Range(50,100));				
//				enemiesInWave.Add(speedyEnemy);
				GameObject homingEnemy0 = new GameObject("HomingEnemy");
				homingEnemy0.AddComponent<Homing>();
				homingEnemy0.transform.position = Vector3.forward * Random.Range(50,100) + new Vector3 (Random.Range(-9,9), 0, Random.Range(50,100));				
				enemiesInWave.Add(homingEnemy0);
			break;
			case 3: //boss
				GameObject boss = new GameObject("Boss");
				Debug.Log("Boss spawned!");
				boss.AddComponent<Boss>();
//				boss.GetComponent<Boss>().MyBehaviorPattern();
				enemiesInWave.Add(boss);
			break;
			default:
			break;
		}
	}

	public void PopulateWave(int _enemyId, int _numEnemies){
		switch (_enemyId){
			case 0:
			for (int i = 0; i<_numEnemies; i++){
				GameObject homingEnemy = new GameObject("HomingEnemy");
				homingEnemy.AddComponent<Homing>();
				homingEnemy.transform.position = Vector3.forward * Random.Range(50,100) + new Vector3 (Random.Range(-9,9), 0, Random.Range(50,100));				
				enemiesInWave.Add(homingEnemy);
//				GameObject speedyEnemy = new GameObject ("SpeedyEnemy");
//				speedyEnemy.AddComponent<Speedy>();
//				// speedyEnemy.transform.position = Vector3.zero;
//				speedyEnemy.transform.position = Vector3.forward * Random.Range(50,100) + new Vector3 (Random.Range(-9,9), 0, Random.Range(50,100));				
//				enemiesInWave.Add(speedyEnemy);
 			}
			break;
			case 1: 
			for (int i = 0; i<_numEnemies; i++){
				GameObject sniperEnemy = new GameObject ("SniperEnemy");
				sniperEnemy.AddComponent<Sniper>();
				// sniperEnemy.transform.position = Vector3.zero;
				sniperEnemy.transform.position = Vector3.forward * Random.Range(50,100) + new Vector3 (Random.Range(-9,9), 0, Random.Range(50,100));				
				enemiesInWave.Add(sniperEnemy);
 //				speedyEnemy.AddComponent<Speedy>();
//				// speedyEnemy.transform.position = Vector3.zero;
//				speedyEnemy.transform.position = Vector3.forward * Random.Range(50,100) + new Vector3 (Random.Range(-9,9), 0, Random.Range(50,100));				
//				enemiesInWave.Add(speedyEnemy);
			}
			break;
			case 2: 
				for (int i = 0; i<_numEnemies; i++){
//					GameObject speedyEnemy = new GameObject ("SpeedyEnemy");
//					speedyEnemy.AddComponent<Speedy>();
//					// speedyEnemy.transform.position = Vector3.zero;
//					speedyEnemy.transform.position = Vector3.forward * Random.Range(50,100) + new Vector3 (Random.Range(-9,9), 0, Random.Range(50,100));				
//					enemiesInWave.Add(speedyEnemy);
					GameObject sniperEnemy = new GameObject ("SniperEnemy");
					sniperEnemy.AddComponent<Sniper>();
					// sniperEnemy.transform.position = Vector3.zero;
					sniperEnemy.transform.position = Vector3.forward * Random.Range(50,100) + new Vector3 (Random.Range(-9,9), 0, Random.Range(50,100));				
					enemiesInWave.Add(sniperEnemy);
					GameObject homingEnemy = new GameObject("HomingEnemy");
					homingEnemy.AddComponent<Homing>();
					homingEnemy.transform.position = Vector3.forward * Random.Range(50,100) + new Vector3 (Random.Range(-9,9), 0, Random.Range(50,100));				
					enemiesInWave.Add(homingEnemy);
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
}
