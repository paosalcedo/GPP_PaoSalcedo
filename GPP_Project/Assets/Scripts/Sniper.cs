using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using DG.Tweening;
public class Sniper : SubclassSandbox.Enemy {

	private Tree<Sniper> _tree;

	[SerializeField]private float healthForInspector;
	private float downSpeed;
	private float _attackRange;
	// Use this for initialization
	private bool hasFired;
	protected override void Start () {
		base.Start();
		_attackRange = 30f;
		speed = 0.25f;
		downSpeed = 3f;
		health = 20f;
		damage = 30f;	
		thisMeshFilter.mesh = GetMesh("sniper");
		thisMeshRenderer.material = GetMaterial("SniperMat");
		_tree = new Tree<Sniper>(
			new Selector<Sniper>(
				new Sequence<Sniper>( //if health is low and player is near, get out
					new IsInDanger(),
					new IsPlayerInRange(),
					new FleeToHeal()
				),
				new Sequence<Sniper>( 
					new IsPlayerInRange(),
					new Attack()
				),
				new MoveToPlayer()
			)
		);
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
		_tree.Update(this); 
		healthForInspector = health;
 	}

	protected override void Move(){
//		transform.Translate(transform.right * speed * Time.deltaTime);
//		transform.Translate(Vector3.back * downSpeed * Time.deltaTime);
		transform.Translate(GetPlayerDirection(Player.instance.gameObject) * speed * Time.deltaTime, Space.World);
	}
	protected override void Shoot(){
		
		if(GetDistanceToPlayer(Player.instance.gameObject) <= 30 /*&& Vector3.Angle(GetPlayerDirection(Player.instance.gameObject), -transform.forward) <= 1*/){	
			GameObject bullet = Instantiate(Resources.Load("Prefabs/SniperBullet"), transform.position, Quaternion.identity) as GameObject;
			// hasFired = true;
 		} 
	}
	protected override void ApplyDamage(){
	}

	private void EscapeToHeal(){
		
		transform.position = Player.instance.transform.position + new Vector3(Random.insideUnitCircle.x, 0, Random.insideUnitCircle.y) * 100;
		health = 20;
	}

	private class IsPlayerInRange : Node<Sniper>
    {
        public override bool Update(Sniper sniper)
        {
            var sniperPos = sniper.transform.position;
            return Vector3.Distance(Player.instance.transform.position, sniperPos) < sniper._attackRange;
        }
    }

	private class Attack : Node<Sniper>
    {
        public override bool Update(Sniper sniper)
        {
            sniper.Shoot();
            return true;
        }
    }

	private class MoveToPlayer : Node<Sniper>
    {
        public override bool Update(Sniper sniper)
        {
            sniper.Move();
            return true;
        }
    }

	private class IsInDanger : Node<Sniper>{
		public override bool Update(Sniper sniper){
			return sniper.health < 20;
		}
	}

	private class FleeToHeal : Node<Sniper>{
		public override bool Update(Sniper sniper){
			sniper.EscapeToHeal();
			return true;
		} 
	}
}
