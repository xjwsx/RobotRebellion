using System.Collections;
using UnityEngine;

public class EnemyAimingState : EnemyBaseState
{
    private RangedEnemy rangedEnemy;
    private float aimingTime = 2f;
    private float timer;
    public EnemyAimingState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine){}
    public override void Enter()
    {
        base.Enter();
        rangedEnemy = stateMachine.Enemy.GetComponent<RangedEnemy>();
        stateMachine.Enemy.StartCoroutine(nameof(AimingCoroutine));
    }
    public override void Exit() 
    { 
        base.Exit();
        rangedEnemy.DeactivateDangerMarker();
        StopAnimation(stateMachine.Enemy.AnimationData.IdleParamaterHash);
        StopAnimation(stateMachine.Enemy.AnimationData.AttackParamaterHash);
        StopAnimation(stateMachine.Enemy.AnimationData.WalkParamaterHash);
    }
    public override void Execute()
    {
        base.Execute();
    }
    private IEnumerator AimingCoroutine()
    {
        timer = 0f;
        while (timer < aimingTime)
        {
            if (rangedEnemy.lookAtPlayer)
            {
                rangedEnemy.ShowDangerMarker();
            }
            timer += Time.deltaTime;
            yield return null;
        }
        stateMachine.ChangeState(stateMachine.AttackState);
    }
}
