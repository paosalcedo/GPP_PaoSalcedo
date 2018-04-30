using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Word : MonoBehaviour
{
	private FSM<Word> _fsm;
	private bool hasCollider;

	private GameObject wordsHolder;
	// Use this for initialization
	void Start ()
	{
		wordsHolder = GameObject.Find("Words");
		transform.SetParent(wordsHolder.transform);
		_fsm = new FSM<Word>(this);
		_fsm.TransitionTo<LookingAtPlayer>();
	}
	
	// Update is called once per frame
	void Update () {
		_fsm.Update();
		//Look at player
//		if (GetPlayerDistance() > 100f)
//		{
//			LookAtPlayer();
//		}
//
//
//		TurnOnColliderWhenPlayerIsNear();
	}

	private bool isTweenActive;
	
	private void SlowLookAtTween()
	{
 //		Sequence sequence = DOTween.Sequence();
//		sequence.Append(transform.DORotate(2 * transform.position - Player.instance.gameObject.transform.position, 5f));
//		sequence.OnComplete(() => isTweenActive = false);

		transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles,
			2 * transform.position - Player.instance.gameObject.transform.position, Time.time);
	}

	private void TurnOnColliderWhenPlayerIsNear()
	{
		if (GetPlayerDistance() <= 30f)
		{
			if (!hasCollider)
			{
				GenerateCollider();
			}
		}
		else if (GetPlayerDistance() > 30f)
		{
			if (hasCollider)
			{
				Destroy(gameObject.GetComponent<BoxCollider>());
				hasCollider = false;
			}
		}
	}

	private void GenerateCollider()
	{
		gameObject.AddComponent<BoxCollider>();
		BoxCollider boxCollider = GetComponent<BoxCollider>();
		boxCollider.size *= 2f;
		boxCollider.size = new Vector3(boxCollider.size.x, boxCollider.size.y, 20f);
		hasCollider = true;
	}

	private void LookAtPlayer()
	{
		transform.LookAt(2 * transform.position - Player.instance.gameObject.transform.position);
	}

	private float GetPlayerDistance()
	{
		float playerDist;
		playerDist = Vector3.Distance(Player.instance.transform.position, transform.position);
		return playerDist;
	}

	private class WordState : FSM<Word>.State
	{
		
	}

	private class LookingAtPlayer : WordState
	{
		public override void OnEnter()
		{
			base.OnEnter();	
//			Context.LookAtPlayer();
		}

		public override void Update()
		{
			base.Update();
			if (Context.GetPlayerDistance() > 100)
			{
 				Context.LookAtPlayer();
			}
			else
			{
				TransitionTo<ReadyForCollision>();	
			}
		}
	}

	private class ReadyForCollision : WordState
	{
		public override void OnEnter()
		{
			base.OnEnter();
		}

		public override void Update()
		{
			base.Update();
			Context.TurnOnColliderWhenPlayerIsNear();

			if (Context.GetPlayerDistance() >= 100)
			{
				TransitionTo<LookingAtPlayer>();
			}
		}

		public override void OnExit()
		{
			base.OnExit();
 			Context.transform.DORotateQuaternion(Quaternion.LookRotation(Context.transform.position - Player.instance.gameObject.transform.position), 1f);
 		}
	}

}
