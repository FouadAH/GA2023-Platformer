using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBeetle_MoveState : MoveState
{
    BlueBeetleEntity beetleEntity;

    public BlueBeetle_MoveState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_MoveState stateData, BlueBeetleEntity blueBeetleEntity) : base(etity, stateMachine, animBoolName, stateData)
    {
        beetleEntity = blueBeetleEntity;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isDetectingWall || !isDetectingLedge)
        {
            beetleEntity.idleState.SetFlipAfterIdle(true);
            stateMachine.ChangeState(beetleEntity.idleState);
        }
    }
}
