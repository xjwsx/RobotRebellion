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

        //UseSkills();
    }
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.AttackParamaterHash);
    }
    //public override void Execute()
    //{
    //    base.Execute();
    //    if (!IsInAttackRange())
    //    {
    //        stateMachine.ChangeState(stateMachine.IdleingState);
    //    }
    //}
    private void UseSkills()
    {
        foreach (SkillType skill in stateMachine.Player.Skills)
        {
            switch (skill)
            {
                case SkillType.BounceAttack:
                    if (stateMachine.Player.PlayerTargeting.targetIndex != -1)
                    {
                        stateMachine.Player.PlayerTargeting.BounceAttack(stateMachine.Player.PlayerTargeting.MonsterList[stateMachine.Player.PlayerTargeting.targetIndex]);
                    }
                    break;
                case SkillType.DoubleBullet:
                    stateMachine.Player.PlayerTargeting.DoubleBulletAttack();
                    break;
                case SkillType.TripleBullet:
                    stateMachine.Player.PlayerTargeting.TripleBulletAttack();
                    break;
            }
        }
    }
}
