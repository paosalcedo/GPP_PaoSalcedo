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
	public GameObject myModel;
	
	private Vector3 lookVector;
	private Vector3 moveVector;
	private float myRotation = 0;
	private float lookSensitivity = 0.1f;
	float jetModelRotationZ = 0;
 	float t = 0;
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
 	}
	
	void GetInput(){
		// moveVector.x = player.GetAxis("Move Vertical");
		InputActions.moveVector.z = player.GetAxis("Move Vertical");
		InputActions.moveVector.x = player.GetAxis("Move Horizontal");
		InputActions.rotation = player.GetAxis("Rotate Ship");
		lookVector.x = player.GetAxis("Rotate Ship");
		InputActions.fire = player.GetButtonDown("Fire");
		InputActions.restart_game = player.GetButtonDown("Restart");
		// InputActions.i_fire =  
	}

	void ProcessInput(){
		//Movement
		transform.Translate(InputActions.moveVector * moveSpeed * Time.deltaTime);
	
		//Shooting
		if(InputActions.fire){
			Debug.Log("Firing!");
			CreateBullet();
		}

		//Ship Rotation
		transform.Rotate (0, lookVector.x * lookSensitivity, 0);
		//only rotate the ship model's zRotation
		jetModelRotationZ = -InputActions.moveVector.x * 45;
   		myModel.transform.localEulerAngles = new Vector3(0, 0, jetModelRotationZ);
		
		
//		if (InputActions.moveVector.x < 0)
//		{
//			t += Time.deltaTime;
//			zRot = Mathf.Lerp(0, 45f, t);
//		}
//		
//		else if (InputActions.moveVector.x > 0)
//		{
//			t += Time.deltaTime;
//			zRot = Mathf.Lerp(0, -45f, t);
//		}
//		else
//		{
//			t += Time.deltaTime;
//			zRot = Mathf.Lerp(zRot, 0, t);
//		}


		//Restart
		if(InputActions.restart_game){
			SceneManager.LoadScene("Main");
		}
	}

	private void CreateBullet(){
		Projectile projectile = Instantiate(Resources.Load("Prefabs/PlayerBullet"), transform.position, transform.rotation) as Projectile;	
  	}

	// Update is called once per frame

}
