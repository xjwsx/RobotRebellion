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
        stateMachine.Enemy.NvAgent.SetDestination(targetPosition);
        //Vector3 movementDirection = GetMovementDirection();

        //Rotate(movementDirection);
        //Move(movementDirection);
    }
    protected float GetNormalizedTime(Animator animator, string tag)
    {
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (!animator.IsInTransition(0) && currentInfo.IsTag(tag))
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0f;
        }
    }
    private Vector3 GetMovementDirection()
    {
        return (stateMachine.Target.transform.position - stateMachine.Enemy.transform.position).normalized;
    }
    private void Move(Vector3 direction)
    {
        stateMachine.Enemy.Rigidbody.MovePosition(stateMachine.Enemy.Rigidbody.position + direction * stateMachine.MovementSpeed * Time.deltaTime);
    }

    private void Rotate(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            direction.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            stateMachine.Enemy.transform.rotation = Quaternion.Slerp(stateMachine.Enemy.transform.rotation, targetRotation, stateMachine.RotationDamping * Time.deltaTime);
        }
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
