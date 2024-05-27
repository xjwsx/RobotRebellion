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
    }

    public override void Exit() 
    { 
        base.Exit();
        StopAnimation(stateMachine.Enemy.AnimationData.AttackParamaterHash);
    }

    public override void Execute()
    {
        base.Execute();
    }
}
