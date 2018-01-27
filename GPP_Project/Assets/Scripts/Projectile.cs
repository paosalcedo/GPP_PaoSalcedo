using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	float speed = 10;
	float damage = 10;

	void Update(){
		transform.Translate(transform.forward * speed * Time.deltaTime);
	}

}
