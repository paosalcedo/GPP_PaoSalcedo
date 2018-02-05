using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager {

	private float waveTime = 0;
	public static EnemyManager enemyManager { get; set; }
	public enum EnemyType{
		Homing = 0,
		Sniper = 1
	}
	public Dictionary<EnemyType, SubclassSandbox.Enemy> enemies = new Dictionary<EnemyType, SubclassSandbox.Enemy>() {
		{ EnemyType.Homing, Resources.Load("Prefabs/Homing") as Homing },
		{ EnemyType.Sniper, Resources.Load("Prefabs/Sniper") as Sniper }
	};
	public List<SubclassSandbox.Enemy> enemiesInWave = new List<SubclassSandbox.Enemy>();
	public List<GameObject> gameObjectsInWave = new List<GameObject>();
	public void Start () {
		PopulateWave(0, 10);
		Debug.Log(enemiesInWave.Count);
	}
	
	// Update is called once per frame
	public void Update () {
		if(enemies.Count<=0){
			enemies.Clear();
			// PopulateWave();
			// EmitWave();
		}

		if(Input.GetKeyDown(KeyCode.Return)){
			PopulateWave(0, 10);
		}

		if(Input.GetKeyDown(KeyCode.P)){
			PopulateWave(1, 15);
		}
	}

	void EmitWave(){
		for (int i = 0; i<enemiesInWave.Count; ++i){
			// GameObject homing = new GameObject()			
		}
	}

	void PopulateWave(int _enemyId, int _numEnemies){
		switch (_enemyId){
			case 0:
			for (int i = 0; i<_numEnemies; i++){
				GameObject homingEnemy = new GameObject("homing");
				homingEnemy.AddComponent<Homing>();
				homingEnemy.transform.position = Player.instance.transform.position + Player.instance.transform.forward * 20f;
				enemiesInWave.Add(homingEnemy.GetComponent<Homing>());
			}
			break;
			case 1: 
			for (int i = 0; i<_numEnemies; i++){
				enemiesInWave.Add(enemies[EnemyType.Homing]);
			}
			break;
			default:
			break;
		}
	}

	void WaveTimer(){
	}
}
