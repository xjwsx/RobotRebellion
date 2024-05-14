using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    public EnemyAttackState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine){}

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Enemy.AnimationData.AttackParamaterHash);
        StopAnimation(stateMachine.Enemy.AnimationData.IdleParamaterHash);
        StopAnimation(stateMachine.Enemy.AnimationData.WalkParamaterHash);
        StopAnimation(stateMachine.Enemy.AnimationData.GetHitParamaterHash);
    }

    public override void Exit() 
    { 
        base.Exit();
        StopAnimation(stateMachine.Enemy.AnimationData.AttackParamaterHash);
    }

    public override void Execute()
    {
        base.Execute();
        if(IsInChaseRange())
        {
            stateMachine.ChangeState(stateMachine.WalkingState);
            return;
        }
        else
        {
            stateMachine.ChangeState(stateMachine.IdleingState);
            return;
        }
    }
}
