using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseState : IState
{
    protected PlayerStateMachine stateMachine;
    public PlayerBaseState(PlayerStateMachine playerstateMachine)
    {
        stateMachine = playerstateMachine;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Execute() 
    {
        Move();
    }
    protected void StartAnimation(int animationHash)
    {
        stateMachine.Player.Animator.SetBool(animationHash, true);
    }

    protected void StopAnimation(int animationHash)
    {
        stateMachine.Player.Animator.SetBool(animationHash, false);
    }
    private void Move()
    {
        if (JoystickMovement.instance.joyVec.x != 0 || JoystickMovement.instance.joyVec.y != 0)
        {
            Vector3 moveDirection = JoystickMovement.instance.joyVec;
            stateMachine.Player.Rigidbody.velocity = new Vector3(moveDirection.x * stateMachine.Player.Data.MoveSpeed, stateMachine.Player.Rigidbody.velocity.y, moveDirection.y * stateMachine.Player.Data.MoveSpeed);
            stateMachine.Player.Rigidbody.rotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0, moveDirection.y));
            stateMachine.ChangeState(stateMachine.WalkingState);
        }
        else
        {
            if (IsInAttackRange())
            {
                stateMachine.ChangeState(stateMachine.AttackState);
            }
            else
            {
                stateMachine.ChangeState(stateMachine.IdleingState);
            }
        }
    }
    public bool IsInAttackRange()
    {
        if (stateMachine.Target.HealthSystem.IsDead) return false;

        float playerDistanceSqr = (stateMachine.Target.transform.position - stateMachine.Player.transform.position).sqrMagnitude;
        return playerDistanceSqr <= stateMachine.Player.Data.AttackRange * stateMachine.Player.Data.AttackRange;
    }
}
