using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGetHitState : EnemyBaseState
{
    public EnemyGetHitState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Enemy.AnimationData.GetHitParamaterHash);
        StopAnimation(stateMachine.Enemy.AnimationData.IdleParamaterHash);
        StopAnimation(stateMachine.Enemy.AnimationData.WalkParamaterHash);
        StopAnimation(stateMachine.Enemy.AnimationData.AttackParamaterHash);
    }
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Enemy.AnimationData.GetHitParamaterHash);
    }
    public override void Execute()
    {
        base.Execute();
        if (!IsInChaseRange())
        {
            stateMachine.ChangeState(stateMachine.IdleingState);
            return;
        }
        else if (IsInAttackRange())
        {
            stateMachine.ChangeState(stateMachine.AttackState);
            return;
        }
    }
}
