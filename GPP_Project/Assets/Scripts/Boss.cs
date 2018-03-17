using System;
using System.Net.Security;
using NUnit.Framework.Constraints;
using UnityEngine;
using SubclassSandbox;
using UnityEditor.VersionControl;

public class Boss : SubclassSandbox.Enemy
{
    private readonly TaskManager _tm = new TaskManager();
    private EnemyManager _em = null;
    private float fullHealth = 0;
    private float midHealth = 250f;
    private float critHealth = 50f;
    private Projectile projectileThatHitMe;
    private bool isInSecondState;
    private bool isInThirdState;
    public Task currentTask;

    public enum HealthState
    {
        Healthy,
        Mid,
        Crit
    }

    [SerializeField]private HealthState healthState;

    protected override void Start () {
        base.Start();
        
        gameObject.AddComponent<SphereCollider>();
        gameObject.GetComponent<SphereCollider>().isTrigger = true;
        gameObject.GetComponent<SphereCollider>().radius = 1;
        gameObject.AddComponent<Rigidbody>();
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        speed = 0.5f;
        health = 500f;
        fullHealth = health;
        midHealth = health * 0.5f;
        critHealth = health * 0.15f;
        thisMeshFilter.mesh = GetMesh("Boss");
         _em = EnemyManager.enemyManager;
        FirstBehaviorPattern();
    }

    protected override void Update ()
    {
         _tm.Update();
        
        if (health <= fullHealth && health > midHealth)
        {
            healthState = HealthState.Healthy;
        }
        else if (health <= midHealth && health > critHealth)
        {
            healthState = HealthState.Mid;
        }
        else if (health <= critHealth )
        {
            healthState = HealthState.Crit;
        }
        
        switch (healthState)
        {
            case HealthState.Healthy:
                break;
            case HealthState.Mid:
                if (!isInSecondState)
                {
                    SecondBehaviorPattern();
                    isInSecondState = true;
                }
                break;
            case HealthState.Crit:
                if (!isInThirdState)
                {
                    ThirdBehaviorPattern();
                    isInThirdState = true;
                }
                break;
            default:
                break;
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
            .Then(new Scale(gameObject, startScale, endScale, 5f))
            .Then(new Move(gameObject, myStartPos, myEndPos, 2f))
            .Then(new BasicAttack(gameObject, 1f))
            .Then(new BasicAttack(gameObject, 1f))
            .Then(new BasicAttack(gameObject, 1f))
            .Then(new BasicAttack(gameObject, 1f))
            .Then(new BasicAttack(gameObject, 1f))
            .Then(new BasicAttack(gameObject, 1f))
            .Then(new Move(gameObject, myEndPos, myStartPos, 2f))
            .Then(new BasicAttack(gameObject, 1f))
            .Then(new BasicAttack(gameObject, 1f))
            .Then(new BasicAttack(gameObject, 1f))
            .Then(new BasicAttack(gameObject, 1f))
            .Then(new BasicAttack(gameObject, 1f))
            .Then(new BasicAttack(gameObject, 1f))
            .Then(new Scale(gameObject, endScale, startScale, 5f))
//            .Then(new ActionTask(FirstBehaviorPattern))
            .Then(new ActionTask(EndFirstBehavior));
//            .Then(new ActionTask(SecondBehaviorPattern))
//            .Then(new ActionTask(ThirdBehaviorPattern));
    }

    
    public void SecondBehaviorPattern()
    {
        if (healthState == HealthState.Mid || healthState == HealthState.Crit)
        {
            _tm.Do(new SpawnMinions(gameObject, 1, transform.position, 1f))
                .Then(new ActionTask(SecondBehaviorPattern));        
        }
    }

    public void ThirdBehaviorPattern()
    {
        if (healthState == HealthState.Crit)
        {
            _tm.Do(new ChaseAttack(gameObject, 60))
                .Then(new ActionTask(ThirdBehaviorPattern));        
        }
    }
    
    public void EndFirstBehavior()
    {
        if (healthState == HealthState.Healthy)
        {
            _tm.Do(new ActionTask(FirstBehaviorPattern));            
        } 
    }        

    protected override void ReceiveDamage(){
        if (projectileThatHitMe != null)
        {
            health -= projectileThatHitMe.damage;
             projectileThatHitMe.DestroyMe();
        } 
    }

    public void BossAttack()
    {
        Shoot();
    }

    public void ChasePlayer()
    {
        transform.Translate(GetPlayerDirection(Player.instance.gameObject) * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Projectile>() != null)
        {
            projectileThatHitMe = other.gameObject.GetComponent<Projectile>();
            ReceiveDamage();
         }

    }
    
//    void OnCollisionEnter(Collision other)
//    {
//        Debug.Log("Something is inside me");
//        projectileThatHitMe = other.gameObject.GetComponent<Projectile>();
//        ReceiveDamage();
//    }

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
    private int _numEnemies = 0;
    private float spawnInterval = 0;
    
    public SpawnMinions(GameObject gameObject, int numEnemies, Vector3 startPosition, float duration) : base(gameObject, duration)
    {
        _numEnemies = numEnemies;
         _startPosition = startPosition;
    }
    
    protected override void OnTick(float t)
    {
        spawnInterval += Time.deltaTime;
        if (spawnInterval >= 1f)
        {
            Boss _boss = gameObject.GetComponent<Boss>();
            EnemyManager.enemyManager.PopulateWaveFromPosition(_numEnemies, _startPosition);
            spawnInterval = 0;
        }
    }
}

public class BasicAttack : TimedGOTask
{
    private float attackInterval = 0.25f;

    public BasicAttack(GameObject gameObject, float duration) : base(gameObject, duration)
    {
     }
    
    protected override void OnTick(float t)
    {
        attackInterval += Time.deltaTime;
        if (attackInterval >= 0.25f)
        {
            Boss _boss = gameObject.GetComponent<Boss>();
            _boss.BossAttack();
            attackInterval = 0;
        }

    }
}

public class ChaseAttack : TimedGOTask
{
    public ChaseAttack(GameObject gameObject, float duration) : base(gameObject, duration)
    {
        
     }
    
    protected override void OnTick(float t)
    {
        Boss _boss = gameObject.GetComponent<Boss>();
        _boss.ChasePlayer();
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






