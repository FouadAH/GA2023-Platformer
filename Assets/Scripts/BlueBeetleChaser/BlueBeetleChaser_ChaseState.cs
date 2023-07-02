using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBeetleChaser_ChaseState : ChaseState
{
    BlueBeetleChaserEntity BlueBeetleChaserEntity;

    public BlueBeetleChaser_ChaseState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_ChaseState stateData, BlueBeetleChaserEntity blueBeetleChaserEntity) : base(etity, stateMachine, animBoolName, stateData)
    {
        BlueBeetleChaserEntity = blueBeetleChaserEntity;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(Time.time > Time.time + stateData.chaseTime) 
        {
            stateMachine.ChangeState(BlueBeetleChaserEntity.idleState);
        }

        if (isDetectingWall || !isDetectingLedge)
        {
            BlueBeetleChaserEntity.idleState.SetFlipAfterIdle(true);
            stateMachine.ChangeState(BlueBeetleChaserEntity.idleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (isDetectingWall || !isDetectingLedge)
        {
            entity.SetVelocityX(0);
            return;
        }

        Vector2 directionToPlayer = ((Vector2)entity.transform.position - entity.playerRuntimeData.playerPosition).normalized;
        if(Mathf.Sign(directionToPlayer.x) == entity.facingDirection)
        {
            entity.Flip();
        }
        entity.SetVelocity(stateData.chaseSpeed);
    }

}
