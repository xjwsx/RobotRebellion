using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    public EnemyIdleState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine) { }
    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Enemy.AnimationData.IdleParamaterHash);
        StopAnimation(stateMachine.Enemy.AnimationData.GetHitParamaterHash);
        StopAnimation(stateMachine.Enemy.AnimationData.WalkParamaterHash);
        StopAnimation(stateMachine.Enemy.AnimationData.AttackParamaterHash);
    }
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Enemy.AnimationData.IdleParamaterHash);
    }
    public override void Execute()
    {
        base.Execute();

        if(IsInChaseRange())
        {
            stateMachine.ChangeState(stateMachine.WalkingState);
            return;
        }
    }
}
