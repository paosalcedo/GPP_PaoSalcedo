using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SubclassSandbox;
public class GameManager : MonoBehaviour {

    Player player;
    void Awake(){
        
    }

    void Start(){
        player = FindObjectOfType<Player>();
    }

    void Update(){
        // Enemy.Update();
    }

    void InitializeEnemies(){
        //  Sandbox.Enemy = new Enemy(); 
    }
}