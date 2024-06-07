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
    public int Exp { get; }
    public float CriticalChance { get; private set; }
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
        Exp = player.Data.Exp;
        CriticalChance = player.Data.CriticalChance;
    }
    public void UpdateTarget()
    {
        if (Player.PlayerTargeting.MonsterList != null)
            Target = FindNearestMonster();
        else
            return;
    }

    private EnemyController FindNearestMonster()
    {
        EnemyController nearestMonster = null;
        float minDistance = float.MaxValue;

        foreach (var monster in Player.PlayerTargeting.MonsterList)
        {
            float distance = Vector3.Distance(Player.transform.position, monster.transform.position);
            if (distance < minDistance)
            {
                nearestMonster = monster.GetComponent<EnemyController>();
                minDistance = distance;
            }
        }

        return nearestMonster;
    }
}
