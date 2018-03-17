using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : SubclassSandbox.Enemy {

	public Vector3 position;

	private float homingTime;
	// Use this for initialization
	protected override void Start () {
		base.Start();
		homingTime = 1f;
		speed = 10f;
		health = 0;
		damage = 20f;
		Destroy(gameObject, 3f);
		audioSource.clip = GetAudioClip("sniper");
		audioSource.volume = 0.02f;
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

	protected override void Move()
	{
		homingTime -= Time.deltaTime;
		if (homingTime > 0)
		{
			transform.Translate((GetPlayerDirection(Player.instance.gameObject) + Random.insideUnitSphere) * speed * Time.deltaTime);		
		}
		else
		{
			transform.Translate(Random.insideUnitCircle * speed * Time.deltaTime);		
		}

	}

	protected override void ApplyDamage(){
		Player.instance.health -= damage;
	}

	protected override void Shoot(){	
	}




}
