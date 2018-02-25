using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Homing : SubclassSandbox.Enemy {
	
	protected override void Start(){
		base.Start();
		speed = 5;
		health = 50;
		damage = 50;
		audioSource.clip = GetAudioClip("explosion_2");
		thisSprite.sprite = GetSprite("homing");
	}
	protected override void Update(){
		base.Update();
 		Move();
		SelfDestructWhenInRangeOfPlayer();
	}

	protected override void Move(){
		// transform.position += Player.instance.transform.position * speed * Time.deltaTime;
		if(GetDistanceToPlayer(Player.instance.gameObject) <= 20){
			transform.Translate(GetPlayerDirection(Player.instance.gameObject) * speed/2.5f * Time.deltaTime, Space.World);
		} else {
			transform.Translate(-transform.forward * speed * Time.deltaTime, Space.World);
		}
		// transform.Translate(Player.instance.transform.position * speed * Time.deltaTime);
		// transform.Translate()
	}

	protected override void Shoot(){
	}

	protected override void ApplyDamage(){
		Player.instance.health -= damage;
	}

	private void SelfDestructWhenInRangeOfPlayer(){
		if(GetDistanceToPlayer(Player.instance.gameObject) <= 2){
			if(!audioSource.isPlaying){
				audioSource.PlayOneShot(audioSource.clip);
			}
			gameObject.GetComponent<SpriteRenderer>().enabled = false;
			Destroy(gameObject, audioSource.clip.length);
		}
	}

	private void OnDestroy(){
		ApplyDamage();
	}
}
