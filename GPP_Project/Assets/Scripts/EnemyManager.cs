using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager {
	public static EnemyManager enemyManager { get; set; }
	public List<GameObject> enemiesInWave = new List<GameObject>();
	public void Start () {
		PopulateWave(0, 10);

	}
	
	// Update is called once per frame
	public void Update () {
		if(enemiesInWave.Count<=0){
			// enemies.Clear();
			PopulateWave(Random.Range(0,2), Random.Range(5,10));
 		}
		Debug.Log(enemiesInWave.Count);
		 
		if(enemiesInWave.Count > 0){
			foreach (var enemy in enemiesInWave){
				SubclassSandbox.Enemy _enemy = enemy.GetComponent<SubclassSandbox.Enemy>();
				if(_enemy.health <= 0){
					enemiesInWave.Remove(_enemy.gameObject);
					_enemy.DestroyMe();
				}
			}
		}
	}

	void PopulateWave(int _enemyId, int _numEnemies){
		switch (_enemyId){
			case 0:
			for (int i = 0; i<_numEnemies; i++){
				GameObject homingEnemy = new GameObject("HomingEnemy");
				homingEnemy.AddComponent<Homing>();
				homingEnemy.transform.position = Vector3.forward * Random.Range(50,100) + new Vector3 (Random.Range(-9,9), 0, Random.Range(50,100));				
				enemiesInWave.Add(homingEnemy);
 			}
			break;
			case 1: 
			for (int i = 0; i<_numEnemies; i++){
				GameObject sniperEnemy = new GameObject ("SniperEnemy");
				sniperEnemy.AddComponent<Sniper>();
				// sniperEnemy.transform.position = Vector3.zero;
				sniperEnemy.transform.position = Vector3.forward * Random.Range(50,100) + new Vector3 (Random.Range(-9,9), 0, Random.Range(50,100));				
				enemiesInWave.Add(sniperEnemy);
			}
			break;
			default:
			break;
		}
	}

	void WaveTimer(){
	}
}
