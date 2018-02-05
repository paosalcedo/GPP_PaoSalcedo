using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SubclassSandbox {
	[RequireComponent(typeof(SpriteRenderer))]
	[RequireComponent(typeof(AudioSource))]
	public abstract class Enemy : MonoBehaviour {
		public float speed { get; set; }
		public float health { get; set; }
		public float damage { get; set; }
		public AudioSource audioSource { get; set; }
		public AudioClip clip { get; set;}
		public SpriteRenderer thisSprite { get; set; }

		protected virtual void Start(){
			audioSource = GetComponent<AudioSource>();
			thisSprite = GetComponent<SpriteRenderer>();
		}
		protected virtual void Update(){
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
		protected float GetDistanceToPlayer(GameObject _player){
			float distanceToPlayer;
			distanceToPlayer = Vector3.Distance(gameObject.transform.position, _player.transform.position);
			return distanceToPlayer;
		}

		protected Vector3 GetPlayerDirection(GameObject _player){
			Vector3 direction;
			direction = _player.transform.position - transform.position; 
		 	return direction;
		 }

		//Sandbox methods
		protected abstract void Move();
		protected abstract void Shoot();
		protected abstract void ApplyDamage();
	}
}
