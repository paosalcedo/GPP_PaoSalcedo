using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

 	public float speed = 30;
	float damage = 10;

	Player player;
	void Start(){
		player = FindObjectOfType<Player>();
		speed = player.GetComponent<Rigidbody>().velocity.magnitude + speed;
	}
	void Update(){
		transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
		// transform.
	}

}
