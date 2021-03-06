﻿using System.Collections;
using System.Collections.Generic;
using SubclassSandbox;
using UnityEngine;

namespace SubclassSandbox {
	[RequireComponent(typeof(MeshFilter))]
	[RequireComponent(typeof(MeshRenderer))]
	[RequireComponent(typeof(AudioSource))]
	public abstract class Enemy : MonoBehaviour {
		public float speed { get; set; }
		public float health { get; set; }
		public float damage { get; set; }
		public AudioSource audioSource { get; set; }
		public AudioClip clip { get; set;}
		public SpriteRenderer thisSprite { get; set; }
		public MeshFilter thisMeshFilter { get; set; }
		public MeshRenderer thisMeshRenderer { get; set; }
		protected virtual void Start()
		{
			audioSource = GetComponent<AudioSource>();
			thisSprite = GetComponent<SpriteRenderer>();
			thisMeshFilter = GetComponent<MeshFilter>();
			thisMeshRenderer = GetComponent<MeshRenderer>();
		}
		protected virtual void Update(){
			ReceiveDamage();
			if (health <= 0)
			{
				DestroyMe();
			}
		}

		//tool methods
		protected AudioClip GetAudioClip(string _fileName){
			AudioClip myAudio;
			myAudio = Resources.Load<AudioClip>("Audio/" + _fileName);
			return myAudio;
		}
		protected Sprite GetSprite(string _fileName){
			Sprite mySprite;
			mySprite = Resources.Load<Sprite>("Sprites/" + _fileName);
			return mySprite; 
		}

		protected Mesh GetMesh(string _fileName)
		{
			Mesh myMesh;
			myMesh = Resources.Load<Mesh>("Models/" + _fileName);
			return myMesh; 
		}

		protected Material GetMaterial(string _fileName)
		{
			Material myMaterial;
			myMaterial = Resources.Load<Material>("Materials/" + _fileName);
			return myMaterial;
		}

		protected float GetDistanceToPlayer(GameObject _player){
			float distanceToPlayer;
			distanceToPlayer = Vector3.Distance(gameObject.transform.position, _player.transform.position);
			return distanceToPlayer;
		}

		protected float GetDistanceToProjectile(Projectile _projectile){
			float distanceToProjectile = 0;
			distanceToProjectile = Vector3.Distance(gameObject.transform.position, _projectile.pos);
			return distanceToProjectile;	
		}

		protected Vector3 GetPlayerDirection(GameObject _player){
			Vector3 direction;
			direction = _player.transform.position - transform.position; 
		 	return direction;
	 	}

		protected bool IsAlive()
		{
			if (health <= 0)
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		//Sandbox methods
		protected abstract void Move();
		protected abstract void Shoot();
		protected abstract void ApplyDamage();

		protected virtual void ReceiveDamage(){
			Projectile[] allProjectiles = FindObjectsOfType<Projectile>();
			foreach (var projectile in allProjectiles){
				if(GetDistanceToProjectile(projectile) <= 1f){
					health -= projectile.damage;
					Debug.Log("You hit " + gameObject.name);
					projectile.DestroyMe();
 				}
			}
		}

		protected virtual void DestroyMe(){
			EventManager.Instance.Fire(new EnemyDeath());
			Destroy(gameObject);
		}

	}
}

public class EnemyDeath : GameEvent{

}