using System;
using System.Collections;
using System.Collections.Generic;
using SubclassSandbox;
using UnityEngine;
using UnityEngine.Video;

public class Speedy : SubclassSandbox.Enemy {

	protected override void Start(){
		base.Start();
		speed = 50;
		health = 20;
		damage = 30;
		thisSprite.sprite = GetSprite("speedy");
		EventManager.Instance.Register<EnemyDeath>(SpeedUp);
	}
	protected override void Update(){
		base.Update();
 		Move();
		Shoot();
	}
	
	protected override void Move(){
		// transform.position += Player.instance.transform.position * speed * Time.deltaTime;
		if(GetDistanceToPlayer(Player.instance.gameObject) <= 30)
		{
			Quaternion targetRotation = Quaternion.LookRotation(GetPlayerDirection(Player.instance.gameObject));
			Quaternion myCurrentRot = transform.rotation;

			transform.rotation = Quaternion.Slerp(myCurrentRot, targetRotation, Time.deltaTime);
			transform.Translate(0, 0, 10 * Time.deltaTime);

//			transform.Translate(GetPlayerDirection(Player.instance.gameObject) * speed/2.5f * Time.deltaTime, Space.World);
//			transform.rotation = Quaternion.Slerp(transform.rotation,)
		} else {
			transform.Translate(-transform.forward * speed * Time.deltaTime, Space.World);
		}
		// transform.Translate(Player.instance.transform.position * speed * Time.deltaTime);
		// transform.Translate()
	}

	protected override void Shoot(){
		if(GetDistanceToPlayer(Player.instance.gameObject) <= 30 && Vector3.Angle(GetPlayerDirection(Player.instance.gameObject), -transform.forward) <= 90){	
			GameObject bullet = Instantiate(Resources.Load("Prefabs/SniperBullet"), transform.position, Quaternion.identity) as GameObject;
			// hasFired = true;
		} 
	}

	public override void DestroyMe()
	{
		EventManager.Instance.Unregister<EnemyDeath>(SpeedUp);
		base.DestroyMe();
	}
	
	protected override void ApplyDamage(){
		Player.instance.health -= damage;
	}

	public void SpeedUp(GameEvent e)
	{
		EnemyDeath enemyDeath = e as EnemyDeath;
		speed += 3;
 	}

}
