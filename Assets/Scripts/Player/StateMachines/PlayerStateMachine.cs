using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    public PlayerController Player { get; }
    public EnemyController Target { get; private set; }
    public PlayerIdleState IdleingState { get;}
    public PlayerWalkState WalkingState { get;}
    public PlayerAttackState AttackState { get;}
    public float AttackRange { get; private set; }
    public float MaxHp { get; private set; }
    public float MovementSpeed { get; private set; }
    public int Damage { get; private set; }
    public PlayerStateMachine(PlayerController player)
    {
        Player = player;
        Target = GameObject.FindGameObjectWithTag("Monster").GetComponent<EnemyController>();

        IdleingState = new PlayerIdleState(this);
        WalkingState = new PlayerWalkState(this);
        AttackState = new PlayerAttackState(this);

        AttackRange = player.Data.AttackRange;
        MaxHp = player.Data.MaxHp;
        MovementSpeed = player.Data.MoveSpeed;
        Damage = player.Data.Damage;
    }
}
