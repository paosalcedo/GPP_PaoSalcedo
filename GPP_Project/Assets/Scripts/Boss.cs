using System;
using System.Collections.Generic;
using System.Net.Security;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Metadata;
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
     
    //healthy phase tasks
    private Task growTask;
    private Task shrinkTask;
    private Task attackTask;
    private Task attackTask1;
    private List<Task> healthyTaskList = new List<Task>();
    private List<Task> midTaskList = new List<Task>();
    private List<Task> critTaskList = new List<Task>();
     
    public enum HealthState
    {
        Healthy,
        Mid,
        Crit
    }

    [SerializeField]private HealthState healthState;

    protected override void Start () {
        base.Start();
        //initialize the tasks
        growTask = new Scale(gameObject, Vector3.one, Vector3.one * 5, 5f);
        shrinkTask = new Scale(gameObject, Vector3.one * 5, Vector3.one, 5f);
        attackTask = new BasicAttack(gameObject, 1f);
        attackTask1 = new BasicAttack(gameObject, 1f);
        
        healthyTaskList.Add(growTask);
        healthyTaskList.Add(shrinkTask);
        healthyTaskList.Add(attackTask);
        healthyTaskList.Add(attackTask1);
        
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
        thisMeshRenderer.material = GetMaterial("BossMat");
        
         _em = EnemyManager.enemyManager;
        
        FirstBehaviorPattern();
    }

    
 
    protected override void Update ()
    {
         _tm.Update();

//        if (Input.GetKeyDown(KeyCode.P))
//        {
//            foreach (var task in healthyTaskList)
//            {
//                task.Abort();
//            }
//
//            foreach (var task in midTaskList)
//            {
//                task.Abort();
//            }
//
//            foreach (var task in critTaskList)
//            {
//                task.Abort();
//            }
//        }

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
                    
                    foreach (var task in healthyTaskList)
                    {
                        task.Abort();   
                    }
                    SecondBehaviorPattern();
                    isInSecondState = true;
                }
                break;
            case HealthState.Crit:
                if (!isInThirdState)
                {
                    foreach (var task in midTaskList)
                    {
                        task.Abort();   
                    }
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
        var myStartPos = transform.position;
        var myEndPos = Player.instance.transform.position + (Vector3.forward * 10f) + (Vector3.left * 10f);
        var myMidPos = Player.instance.transform.position + Vector3.one;
        
        var startScale = Vector3.one;
        var endScale = startScale * 5;
        
        _tm.Do(new SetPos(gameObject, Vector3.zero))
            .Then(growTask)
            .Then(attackTask)
            .Then(MoveTask(myStartPos, myEndPos))
            .Then(attackTask1)
            .Then(shrinkTask)
            .Then(MoveTask(myEndPos, myStartPos))
            .Then(new ActionTask(FirstBehaviorPattern));
     }

    
    public void SecondBehaviorPattern()
    {
        if (healthState == HealthState.Mid || healthState == HealthState.Crit)
        {
            _tm.Do(SpawnMinionsTask())
                .Then(new ActionTask(SecondBehaviorPattern));        
        }
    }

    public void ThirdBehaviorPattern()
    {
        if (healthState == HealthState.Crit)
        {
            _tm.Do(ChaseTask())
                .Then(new ActionTask(ThirdBehaviorPattern));
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
    
    private Task MoveTask(Vector3 pos1, Vector3 pos2)
    {
        Task someTask;
        someTask = new Move(gameObject, pos1, pos2, 2f);
        healthyTaskList.Add(someTask);
        return someTask;
    }

    private Task SpawnMinionsTask()
    {
        Task someTask;
        someTask = new SpawnMinions(gameObject, 1, transform.position, 5);
        midTaskList.Add(someTask);
        return someTask;
    }

    private Task ChaseTask()
    {
        Task someTask;
        someTask = new ChaseAttack(gameObject, 60f);
        critTaskList.Add(someTask);
        return someTask;
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
            EnemyManager.enemyManager.SpawnBossMinionWave(_numEnemies, _startPosition);
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
    private float attackInterval = 0.25f;

    public ChaseAttack(GameObject gameObject, float duration) : base(gameObject, duration)
    {
        Boss _boss = gameObject.GetComponent<Boss>();
      }
    
    protected override void OnTick(float t)
    {
        Boss _boss = gameObject.GetComponent<Boss>();
        _boss.ChasePlayer();
        attackInterval += Time.deltaTime;
        if (attackInterval >= 0.25f)
        {
             _boss.BossAttack();
            attackInterval = 0;
        }
    }
}







