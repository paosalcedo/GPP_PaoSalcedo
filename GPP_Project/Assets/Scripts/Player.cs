using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Player : MonoBehaviour {

	[SerializeField]float moveSpeed;
	Rewired.Player player;
	private int playerId = 0;

	private Vector3 moveVector;
	Rigidbody rb;

	// Use this for initialization
	void Start () {
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
		InputActions.fire = player.GetButton("Fire");
		// InputActions.i_fire =  
	}

	void ProcessInput(){
		#region Movement
		rb.velocity = InputActions.moveVector * moveSpeed;
		#endregion

		#region Shooting

		#endregion
	}

	// Update is called once per frame

}
