using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SubclassSandbox;
public class Drone : Enemy{
    private FSM<Drone> _fsm;

    private void Start()
    {
        _fsm = new FSM<Drone>(this);
        base.Start();
        speed = 5;
        health = 50;
        damage = 50;
        thisMeshFilter.mesh = GetMesh("homing");
        thisMeshRenderer.material = GetMaterial("HomingMat");
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

    //STATES
    private class DroneState : FSM<Drone>.State {
        
    }
}
