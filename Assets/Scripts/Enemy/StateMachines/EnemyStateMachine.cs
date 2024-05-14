using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : StateMachine
{
    public EnemyController Enemy { get; }
    public PlayerController Target { get; private set; }
    public EnemyIdleState IdleingState { get; }
    public EnemyWalkState WalkingState { get;}
    public EnemyAttackState AttackState { get; }
    public EnemyGetHitState GetHitState { get; }
    public string MonsterName { get; private set; }
    public float ChasingRange { get; private set; }
    public float AttackRange { get; private set; }
    public float MaxHp { get; private set; }
    public float MovementSpeed { get; private set; }
    public int Damage {  get; private set; }
    public float RotationDamping { get; private set; }


    public EnemyStateMachine(EnemyController enemy)
    {
        Enemy = enemy;
        Target = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        IdleingState = new EnemyIdleState(this);
        WalkingState = new EnemyWalkState(this);
        AttackState = new EnemyAttackState(this);
        GetHitState = new EnemyGetHitState(this);

        MonsterName = enemy.Data.MonsterName;
        ChasingRange = enemy.Data.PlayerChasingRange;
        AttackRange = enemy.Data.AttackRange;
        MaxHp = enemy.Data.MaxHp;
        MovementSpeed = enemy.Data.MoveSpeed;
        Damage = enemy.Data.Damage;
        RotationDamping = enemy.Data.BaseRotationDamping;
    }

}
