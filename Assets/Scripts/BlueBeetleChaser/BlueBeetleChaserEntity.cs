using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBeetleChaserEntity : Entity
{
    public IdleState idleState { get; private set; }
    public MoveState moveState { get; private set; }
    public ChaseState chaseState { get; private set; }


    [Header("States")]
    public D_IdleState idleStateData;
    public D_MoveState moveStateData;
    public D_ChaseState chaseStateData;


    public override void Start()
    {
        base.Start();

        moveState = new BlueBeetleChaser_MoveState(this, stateMachine, "move", moveStateData, this);
        idleState = new BlueBeetleChaser_IdleState(this, stateMachine, "idle", idleStateData, this);
        chaseState = new BlueBeetleChaser_ChaseState(this, stateMachine, "idle", chaseStateData, this);
        stateMachine.Initialize(idleState);
    }
}
