using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class BlueBeetleEntity : Entity
{
    public IdleState idleState { get; private set; }
    public MoveState moveState { get; private set; }

    [Header("States")]
    public D_IdleState idleStateData;
    public D_MoveState moveStateData;

    public override void Start()
    {
        base.Start();

        moveState = new BlueBeetle_MoveState(this, stateMachine, "move", moveStateData, this);
        idleState = new BlueBeetle_IdleState(this, stateMachine, "idle", idleStateData, this);

        stateMachine.Initialize(idleState);
    }
}
