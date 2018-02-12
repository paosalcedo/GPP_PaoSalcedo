using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : SubclassSandbox.Enemy {

	public Vector3 position;
	// Use this for initialization
	protected override void Start () {
		base.Start();
		speed = 100f;
		health = 0;
		damage = 20f;
		Destroy(gameObject, 3f);
		audioSource.clip = GetAudioClip("sniper");
		audioSource.Play();
		thisSprite.sprite = GetSprite("new_bullet");
	}
	
	// Update is called once per frame
	protected override void Update () {
		Move();
		if(GetDistanceToPlayer(Player.instance.gameObject) <= 1f){
			ApplyDamage();
			Destroy(gameObject);
		}
	}

	protected override void Move(){
		transform.Translate(Vector3.back * speed * Time.deltaTime);
	}

	protected override void ApplyDamage(){
		Player.instance.health -= damage;
	}

	protected override void Shoot(){	
	}




}
