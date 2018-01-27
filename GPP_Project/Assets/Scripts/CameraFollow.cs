using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
	float t;
	Camera myCam;
	Player player;
 	void Start () {
		myCam = GetComponent<Camera>();
		player = FindObjectOfType<Player>();
	}
	
	void FixedUpdate(){
		FollowPlayer();
	}

	void FollowPlayer(){
		if(	myCam.WorldToScreenPoint(player.transform.position).x < Screen.width * 0.49f 
			|| myCam.WorldToScreenPoint(player.transform.position).x > Screen.width * 0.51f
			|| myCam.WorldToScreenPoint(player.transform.position).y < Screen.height * 0.49f
			|| myCam.WorldToScreenPoint(player.transform.position).y > Screen.height * 0.51f
		){
			t += 0.06f * Time.deltaTime;
			transform.position = new Vector3(Mathf.Lerp(transform.position.x, player.transform.position.x, t), transform.position.y, Mathf.Lerp(transform.position.z, player.transform.position.z, t));
		} else {
			t = 0;
		}
	}
}
