using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using NUnit.Framework.Internal;

public class Homing : SubclassSandbox.Enemy
{

	private FSM<Homing> _fsm;
	
	private Sequence pulseSequence;

	protected override void Start()
	{
		base.Start();
 		_fsm = new FSM<Homing>(this);
		_fsm.TransitionTo<Seeking>();
		speed = 0.25f;
		health = 50;
		damage = 50;
		audioSource.clip = GetAudioClip("explosion_2");
		thisMeshFilter.mesh = GetMesh("homing");
		thisMeshRenderer.material = GetMaterial("HomingMat");
	}

	protected override void Update()
	{
		base.Update();
		_fsm.Update();
// 		Move();
//		SelfDestructWhenInRangeOfPlayer();
	}

	protected override void Move()
	{
		// transform.position += Player.instance.transform.position * speed * Time.deltaTime;
		transform.Translate(GetPlayerDirection(Player.instance.gameObject) * speed / 2.5f * Time.deltaTime, Space.World);

		// transform.Translate(Player.instance.transform.position * speed * Time.deltaTime);
		// transform.Translate()
	}

	protected override void Shoot()
	{
	}

	protected override void ApplyDamage()
	{
		Player.instance.health -= damage;
	}
	
	
	private void SelfDestructWhenInRangeOfPlayer()
	{
		if (GetDistanceToPlayer(Player.instance.gameObject) <= 2)
		{
			if (!audioSource.isPlaying)
			{
				audioSource.PlayOneShot(audioSource.clip);
			}

			gameObject.GetComponent<MeshRenderer>().enabled = false;
			DestroyMe();
//			Destroy(gameObject, audioSource.clip.length);
		}
	}

	private void OnDestroy()
	{
		ApplyDamage();
	}

	private class HomingState : FSM<Homing>.State
	{
		
	}

	private class Seeking : HomingState
	{
		public override void Update()
		{
			if (Context.GetDistanceToPlayer(Player.instance.gameObject) >= 50f)
			{
				Context.Move(); 				// move toward the player
			}
			else
			{
				TransitionTo<Pulsing>();
				return;
			}
		}
	}

	private class Pulsing : HomingState
	{
		public override void OnEnter()
		{
			Context.pulseSequence = DOTween.Sequence();
			Context.pulseSequence.Append(Context.transform.DOScale(Vector3.one * 5f, 0.5f).SetEase(Ease.InOutSine));
			Context.pulseSequence.Append(Context.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack));
			Context.pulseSequence.SetLoops(5);
 			Context.pulseSequence.OnComplete(()=>TransitionTo<Attacking>());
		}

		public override void Update()
		{
			OnDamageReceived();
		}
		
		private void OnDamageReceived(){
			Projectile[] allProjectiles = FindObjectsOfType<Projectile>();
			foreach (var projectile in allProjectiles){
				if(Context.GetDistanceToProjectile(projectile) <= 1f)
				{
					Context.pulseSequence.Kill();
					TransitionTo<Retreating>();
				}
			}
		}
	}

	private class Attacking : HomingState
	{
		
		public override void OnEnter()
		{
//			attackDir = Context.GetPlayerDirection(Player.instance.gameObject);
			Context.transform.DOMove(Player.instance.transform.position, 0.75f, false).SetEase(Ease.InOutSine)
				.OnComplete(()=>TransitionTo<Seeking>());
		}

		public override void Update()
		{
			Context.SelfDestructWhenInRangeOfPlayer();	
		}
	}

	private class Retreating : HomingState
	{
		public override void OnEnter()
		{
 			Context.transform.DOMove(Player.instance.transform.position + (new Vector3(Random.insideUnitSphere.x, 0, Random.insideUnitSphere.z) * 250f), 0.75f, false).SetEase(Ease.InOutSine)
				.OnComplete(()=>TransitionTo<Seeking>());
		}
	}
}
