using System;
using NUnit.Framework.Constraints;
using UnityEngine;
using SubclassSandbox;

public class Boss : SubclassSandbox.Enemy
{
    private readonly TaskManager _tm = new TaskManager();
    private EnemyManager _em;
  
    protected override void Start () {
        base.Start();
        speed = 5f;
        health = 500f;
         thisSprite.sprite = GetSprite("boss");
        _em = EnemyManager.enemyManager;
        MyBehaviorPattern();
    }

    protected override void Update ()
    {
         _tm.Update();
        ReceiveDamage();
    }

    protected override void Move()
    {
        
    }

    protected override void Shoot()
    {   
    }

    protected override void ApplyDamage()
    {
    }

    public void MyBehaviorPattern()
    {
        // Just setting up some variables so the task constructors below are a little easier to read...
        var startPos = Camera.main.ViewportToWorldPoint(new Vector3(0, 0.5f, 10));
        var endPos = Camera.main.ViewportToWorldPoint(new Vector3(1, 0.5f, 10));
        var midPos = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10));

        var startScale = Vector3.one;
        var endScale = startScale * 5;

        // Teleport to center.
        _tm.Do(new SetPos(gameObject, midPos))

            // Then scale to 100%
            .Then(new ActionTask(()=>Debug.Log("I'm the bosssssss!!!!!")))
            .Then(new Scale(gameObject, startScale, endScale, 5f))
            .Then(new Move(gameObject, startPos, endPos, 2f))
            .Then(new Wait(5))
             .Then(new Scale(gameObject, endScale, startScale, 5f))
            .Then(new Wait(5))
            .Then(new SpawnMinions(gameObject, 1, transform.position, 1))
            .Then(new ActionTask(MyBehaviorPattern));
    }
    
    protected override void ReceiveDamage(){
        Projectile[] allProjectiles = FindObjectsOfType<Projectile>();
        foreach (var projectile in allProjectiles){
            if(GetDistanceToProjectile(projectile) <= 10f){
                health -= projectile.damage;
                Debug.Log("Boss was hit!");
                projectile.DestroyMe();
            }
        }
    }
}

public class Rotate : TimedGOTask
{
    private readonly Vector3 _startRotation, _endRotation;

    public Rotate(GameObject gameObject, Vector3 startRotation, Vector3 endRotation, float duration) : base(gameObject, duration)
    {
        _startRotation = startRotation;
        _endRotation = endRotation;
    }

    protected override void OnTick(float t)
    {
        gameObject.transform.rotation = Quaternion.Euler(Vector3.Lerp(_startRotation, _endRotation, t));
    }
}

public class SpawnMinions : TimedGOTask
{
    private readonly Vector3 _startPosition;
    
    public SpawnMinions(GameObject gameObject, int numEnemies, Vector3 startPosition, float duration) : base(gameObject, duration)
    {
        Debug.Log("Spawning minions!");
        _startPosition = startPosition;
        EnemyManager.enemyManager.PopulateWaveFromPosition(numEnemies, startPosition);
    }

}






