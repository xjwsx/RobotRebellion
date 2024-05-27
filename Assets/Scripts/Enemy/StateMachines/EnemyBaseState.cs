using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBaseState : IState
{
    protected EnemyStateMachine stateMachine;
    public EnemyBaseState(EnemyStateMachine enemyStateMachine)
    { 
        stateMachine = enemyStateMachine;
    }
    public virtual void Enter() { }

    public virtual void Exit() { }
    public virtual void Execute() 
    {
        Move();
    }
    protected void StartAnimation(int animationHash)
    {
        stateMachine.Enemy.Animator.SetBool(animationHash, true);
    }
    protected void StopAnimation(int animationHash)
    {
        stateMachine.Enemy.Animator.SetBool(animationHash, false);
    }
    
    private void Move()
    {
        Vector3 targetPosition = stateMachine.Target.transform.position;
        
        if (!IsInChaseRange())
        {
            stateMachine.ChangeState(stateMachine.IdleingState);
        }
        else if(IsInAttackRange())
        {
            if(stateMachine.Enemy.Data.IsRanged && !stateMachine.Enemy.isPreparingAttack)
            {
                stateMachine.Enemy.StartCoroutine(stateMachine.Enemy.WaitForPlayer());
                //LookAtPlayer();
            }
            else if(!stateMachine.Enemy.Data.IsRanged)
            {
                stateMachine.ChangeState(stateMachine.AttackState);
                LookAtPlayer();
            }
        }
        else
        {
            stateMachine.ChangeState(stateMachine.WalkingState);
            stateMachine.Enemy.NvAgent.SetDestination(targetPosition);
        }
    }
    private void LookAtPlayer()
    {
        Vector3 direction = (stateMachine.Target.transform.position - stateMachine.Enemy.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        stateMachine.Enemy.transform.rotation = Quaternion.Slerp(stateMachine.Enemy.transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
    protected bool IsInChaseRange()
    {
        if( stateMachine.Target.HealthSystem.IsDead) return false;

        float playerDistanceSqr = (stateMachine.Target.transform.position - stateMachine.Enemy.transform.position).sqrMagnitude;
        return playerDistanceSqr <= stateMachine.Enemy.Data.PlayerChasingRange * stateMachine.Enemy.Data.PlayerChasingRange;
    }
    protected bool IsInAttackRange()
    {
        float playerDistanceSqr = (stateMachine.Target.transform.position - stateMachine.Enemy.transform.position).sqrMagnitude;
        return playerDistanceSqr <= stateMachine.Enemy.Data.AttackRange * stateMachine.Enemy.Data.AttackRange;
    }
}
