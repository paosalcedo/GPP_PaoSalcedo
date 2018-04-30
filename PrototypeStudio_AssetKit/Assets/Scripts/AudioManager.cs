using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public static AudioManager instance;

	private AudioSource ambient;
	private AudioSource music;
	private AudioSource sfx;
	[SerializeField]private AudioClip[] pianoClips;
	[SerializeField]private AudioClip[] ambientClips;
	[SerializeField] private AudioClip[] musicClips;

	private double delay = 0.00001f;
	// Use this for initialization
	void Start ()
	{
		if(instance == null){
			instance = this;
		} else {
			Destroy(gameObject);
		}
		music = gameObject.AddComponent<AudioSource>();
		music.clip = musicClips[Random.Range(0, musicClips.Length)];
		music.Play();	
		music.loop = true;
		sfx = gameObject.AddComponent<AudioSource>();
		sfx.playOnAwake = false;
		sfx.loop = false;
	}
	
	// Update is called once per frame


	public void PlaySFXOnHit()
	{
		sfx.clip = pianoClips[Random.Range(0, pianoClips.Length - 1)];
		sfx.pitch = Random.Range(0.5f, 1.5f);
		sfx.PlayScheduled(AudioSettings.dspTime + 0.00000001f);
	}

}
