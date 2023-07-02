using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBeetleChaser_IdleState : IdleState
{
    BlueBeetleChaserEntity BlueBeetleChaserEntity;

    public BlueBeetleChaser_IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData, BlueBeetleChaserEntity blueBeetleEntity) : base(entity, stateMachine, animBoolName, stateData)
    {
        BlueBeetleChaserEntity = blueBeetleEntity;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isIdleTimeOver)
        {
            stateMachine.ChangeState(BlueBeetleChaserEntity.moveState);
        }
    }
}
