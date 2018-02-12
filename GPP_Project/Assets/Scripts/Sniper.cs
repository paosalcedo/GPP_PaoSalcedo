using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : SubclassSandbox.Enemy {

	private float downSpeed;
	// Use this for initialization
	private bool hasFired;
	protected override void Start () {
		base.Start();
		speed = 5f;
		downSpeed = 3f;
		health = 20f;
		damage = 30f;	
		thisSprite.sprite = GetSprite("sniper");
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
		Move();
		Shoot();
	}

	protected override void Move(){
		transform.Translate(transform.right * speed * Time.deltaTime);
		transform.Translate(Vector3.back * downSpeed * Time.deltaTime);
		if(transform.position.x < -8f){
			speed *= -1f;
		} else if (transform.position.x > 8f){
			speed *= -1f;
		}
	}
	protected override void Shoot(){
		
		if(GetDistanceToPlayer(Player.instance.gameObject) <= 30 && Vector3.Angle(GetPlayerDirection(Player.instance.gameObject), -transform.forward) <= 1){	
			GameObject bullet = Instantiate(Resources.Load("Prefabs/SniperBullet"), transform.position, Quaternion.identity) as GameObject;
			// hasFired = true;
 		} 
	}
	protected override void ApplyDamage(){
	}
}
