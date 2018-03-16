using System;
using NUnit.Framework.Constraints;
using UnityEngine;
using SubclassSandbox;

public class Boss : SubclassSandbox.Enemy
{
    private readonly TaskManager _tm = new TaskManager();
    private EnemyManager _em;
    private float critHealth = 0;
    protected override void Start () {
        base.Start();
        speed = 5f;
        health = 500f;
        critHealth = health * 0.15f;
        thisMeshFilter.mesh = GetMesh("Boss");
         _em = EnemyManager.enemyManager;
        FirstBehaviorPattern();
    }

    protected override void Update ()
    {
         _tm.Update();
        ReceiveDamage();
        if (health <= critHealth)
        {
            //change behaviour
        }
    }

    protected override void Move()
    {
        
    }

    protected override void Shoot()
    {   
        GameObject bullet = Instantiate(Resources.Load("Prefabs/BossBullet"), transform.position, Quaternion.identity) as GameObject;
    }

    protected override void ApplyDamage()
    {
    }

    public void FirstBehaviorPattern()
    {
        // Just setting up some variables so the task constructors below are a little easier to read...
//        var startPos = Camera.main.ViewportToWorldPoint(new Vector3(0, 0.5f, 10));
//        var endPos = Camera.main.ViewportToWorldPoint(new Vector3(1, 0.5f, 10));
//        var midPos = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10));
        var myStartPos = transform.position;
        var myEndPos = Player.instance.transform.position + (Vector3.forward * 10f) + (Vector3.left * 10f);
        var myMidPos = Player.instance.transform.position + Vector3.one;
        
        var startScale = Vector3.one;
        var endScale = startScale * 5;

        // Teleport to center.
        _tm.Do(new SetPos(gameObject, Vector3.zero))

            // Then scale to 100%
            .Then(new Scale(gameObject, startScale, endScale, 5f))
            .Then(new Move(gameObject, myStartPos, myEndPos, 2f))
            .Then(new BasicAttack(gameObject, 1f))
            .Then(new BasicAttack(gameObject, 1f))
            .Then(new BasicAttack(gameObject, 1f))
            .Then(new BasicAttack(gameObject, 1f))
            .Then(new BasicAttack(gameObject, 1f))
            .Then(new BasicAttack(gameObject, 1f))
//             .Then(new Scale(gameObject, endScale, startScale, 5f))
            .Then(new Move(gameObject, myEndPos, myStartPos, 2f))
            .Then(new BasicAttack(gameObject, 1f))
            .Then(new BasicAttack(gameObject, 1f))
            .Then(new BasicAttack(gameObject, 1f))
            .Then(new BasicAttack(gameObject, 1f))
            .Then(new BasicAttack(gameObject, 1f))
            .Then(new BasicAttack(gameObject, 1f))
            .Then(new Scale(gameObject, endScale, startScale, 5f))
//             .Then(new SpawnMinions(gameObject, 1, transform.position, 1))
            .Then(new ActionTask(FirstBehaviorPattern));
    }
    
    protected override void ReceiveDamage(){
//        Projectile[] allProjectiles = FindObjectsOfType<Projectile>();
//        foreach (var projectile in allProjectiles){
//            if(GetDistanceToProjectile(projectile) <= 10f){
//                health -= projectile.damage;
//                Debug.Log("Boss was hit!");
//                projectile.DestroyMe();
//            }
//        }
        Debug.Log("Nothing here!");
    }

    public void BossAttack()
    {
        Shoot();
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
         _startPosition = startPosition;
        EnemyManager.enemyManager.PopulateWaveFromPosition(numEnemies, startPosition);
    }
}

public class BasicAttack : TimedGOTask
{
    public BasicAttack(GameObject gameObject, float duration) : base(gameObject, duration)
    {
//        if (gameObject.GetComponent<Boss>() != null)
//        {
//            Debug.Log("Boss attack!");
//            Boss _boss = gameObject.GetComponent<Boss>();
//            _boss.BossAttack();
//        }
    }
    
    protected override void OnTick(float t)
    {
        Boss _boss = gameObject.GetComponent<Boss>();
        _boss.BossAttack();
    }
}

/*public class Move : TimedGOTask
{
    public Vector3 Start { get; private set; }
    public Vector3 End { get; private set; }

    public Move(GameObject gameObject, Vector3 start, Vector3 end, float duration) : base(gameObject, duration)
    {
        Start = start;
        End = end;
    }

    protected override void OnTick(float t)
    {
        gameObject.transform.position = Vector3.Lerp(Start, End, t);
    }
}*/






