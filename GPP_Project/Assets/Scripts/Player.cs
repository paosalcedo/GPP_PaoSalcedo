using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.SceneManagement;
public class Player : MonoBehaviour {

	[SerializeField]float moveSpeed;
	Rewired.Player player;
	private int playerId = 0;

	private Vector3 moveVector;
	private float myRotation = 0;
	Rigidbody rb;

	// Use this for initialization
	void Start () {
		Cursor.lockState = CursorLockMode.Locked;
		// Cursor.visible = false;
		player = ReInput.players.GetPlayer(playerId);
		rb = GetComponent<Rigidbody>();
	}

	void Update(){
		GetInput();
		ProcessInput();
 	}
	
	void GetInput(){
		// moveVector.x = player.GetAxis("Move Vertical");
		InputActions.moveVector.z = player.GetAxis("Move Vertical");
		InputActions.moveVector.x = player.GetAxis("Move Horizontal");
		InputActions.rotation = player.GetAxis("Rotate Ship");
		InputActions.fire = player.GetButtonDown("Fire");
		InputActions.restart_game = player.GetButtonDown("Restart");
		// InputActions.i_fire =  
	}

	void ProcessInput(){
		//Movement
		rb.velocity = InputActions.moveVector * moveSpeed;
	
		//Shooting
		if(InputActions.fire){
			CreateBullet();
		}

		//Ship Rotation
		myRotation += InputActions.rotation;
		transform.rotation = Quaternion.Euler(0, myRotation, 0);

		//Restart
		if(InputActions.restart_game){
			SceneManager.LoadScene("Main");
		}
	}

	private void CreateBullet(){
		Projectile projectile = Instantiate(Resources.Load("Prefabs/Bullet"), transform.position, transform.rotation) as Projectile;	
	}

	// Update is called once per frame

}
