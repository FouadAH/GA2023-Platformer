using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBeetle_IdleState : IdleState
{
    BlueBeetleEntity beetleEntity;

    public BlueBeetle_IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData, BlueBeetleEntity blueBeetleEntity) : base(entity, stateMachine, animBoolName, stateData)
    {
        beetleEntity = blueBeetleEntity;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isIdleTimeOver)
        {
            stateMachine.ChangeState(beetleEntity.moveState);
        }
    }

}
