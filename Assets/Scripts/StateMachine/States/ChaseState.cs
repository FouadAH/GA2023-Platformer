using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    protected D_ChaseState stateData;

    protected bool isDetectingLedge;
    protected bool isDetectingWall;

    public ChaseState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_ChaseState stateData) : base(etity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isDetectingLedge = entity.CheckLedge();
        isDetectingWall = entity.CheckWallFront();
    }
}
