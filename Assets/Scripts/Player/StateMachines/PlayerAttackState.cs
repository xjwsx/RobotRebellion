using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    public PlayerAttackState(PlayerStateMachine playerstateMachine) : base(playerstateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.AttackParamaterHash);
        StopAnimation(stateMachine.Player.AnimationData.IdleParamaterHash);
        StopAnimation(stateMachine.Player.AnimationData.WalkParamaterHash);
    }
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.AttackParamaterHash);
    }
}
