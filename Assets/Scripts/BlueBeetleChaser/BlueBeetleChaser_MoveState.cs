using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBeetleChaser_MoveState : MoveState
{
    BlueBeetleChaserEntity BlueBeetleChaserEntity;

    public BlueBeetleChaser_MoveState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_MoveState stateData, BlueBeetleChaserEntity blueBeetleEntity) : base(etity, stateMachine, animBoolName, stateData)
    {
        BlueBeetleChaserEntity = blueBeetleEntity;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (entity.CheckPlayerInAggroRange() && !isDetectingWall)
        {
            stateMachine.ChangeState(BlueBeetleChaserEntity.chaseState);
            return;
        }

        if (isDetectingWall || !isDetectingLedge)
        {
            BlueBeetleChaserEntity.idleState.SetFlipAfterIdle(true);
            stateMachine.ChangeState(BlueBeetleChaserEntity.idleState);
        }
    }
}
