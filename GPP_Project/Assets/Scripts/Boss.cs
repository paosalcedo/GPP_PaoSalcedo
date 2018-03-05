using System;
using UnityEngine;
using SubclassSandbox;

public class Boss : MonoBehaviour
{
    private readonly TaskManager _tm = new TaskManager();
    private EnemyManager _em;
    private void Start ()
    {
        _em = EnemyManager.enemyManager;
        DoMyThing();
    }

    private void Update ()
    {
        _tm.Update();
    }

    private void DoMyThing()
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
            .Then(new Scale(gameObject, startScale, endScale, 1f))
            .Then(new SpawnMinions(gameObject, 1, transform.position, 1))
            .Then(new Scale(gameObject, endScale, startScale, 1f))
            // Then reset the whole thing
            .Then(new ActionTask(DoMyThing));
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






