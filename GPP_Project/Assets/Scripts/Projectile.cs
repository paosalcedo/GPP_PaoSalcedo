using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

 	public float speed = 30;
	public float damage = 1;
	public Vector3 pos;
	float lifeTime = 5f;
	Player player;
	void Start(){
		Debug.Log("Player Bullet operational!");
		player = FindObjectOfType<Player>();
		speed = player.GetComponent<Rigidbody>().velocity.magnitude + speed;
	}
	void Update(){
		pos = transform.position;
		transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
		lifeTime -= Time.deltaTime;
		if(lifeTime <= 0){
			Destroy(gameObject);
		}
		// transform.
	}

	public void DestroyMe(){
		Destroy(gameObject);
	}

}
