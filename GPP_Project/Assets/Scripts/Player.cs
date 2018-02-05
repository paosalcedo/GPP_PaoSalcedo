using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.SceneManagement;
public class Player : MonoBehaviour {

	public static Player instance;
	public float health;
	public float shields;
	[SerializeField]float moveSpeed;
	Rewired.Player player;
	private int playerId = 0;

	private Vector3 moveVector;
	private float myRotation = 0;
	Rigidbody rb;

	// Use this for initialization
	void Start () {
		if(instance == null){
			instance = this;
			DontDestroyOnLoad(this);
		} else {
			Destroy(gameObject);
		}
		Cursor.lockState = CursorLockMode.Locked;
		// Cursor.visible = false;
		player = ReInput.players.GetPlayer(playerId);
		rb = GetComponent<Rigidbody>();
	}

	void Update(){
		GetInput();
		ProcessInput();

		if(transform.position.x < -8.2f){
			transform.position = new Vector3(8.2f, 0, transform.position.z);
		} else if (transform.position.x > 8.2f){
			transform.position = new Vector3(-8.2f, 0, transform.position.z);
		}

		if(transform.position.z < -5.5f){
			transform.position = new Vector3(transform.position.x, 0, 25.6f);
		} else if (transform.position.z > 25.6f){
			transform.position = new Vector3(transform.position.x, 0, -5.5f);
		}
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
