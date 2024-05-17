using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [field: Header("References")]
    [field: SerializeField] public EnemySO Data { get; private set; }

    [field: Header("Animations")]
    [field: SerializeField] public EnemyAnimationData AnimationData { get; private set; }
    public Rigidbody Rigidbody { get; private set; }
    public Animator Animator { get; private set; }
    public HealthSystem HealthSystem { get; private set; }
    public NavMeshAgent NvAgent { get; private set; }
    private EnemyStateMachine stateMachine;

    private void Awake()
    {
        AnimationData.Initialize();

        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponent<Animator>();
        HealthSystem = GetComponent<HealthSystem>();
        HealthSystem.SetUp(Data.MaxHp);
        NvAgent = GetComponent<NavMeshAgent>();
        stateMachine = new EnemyStateMachine(this);
    }
    private void Start()
    {
        NavMeshSetting();
        stateMachine.ChangeState(stateMachine.IdleingState);
        HealthSystem.OnDeath += OnDie;
    }
    private void Update()
    {
        stateMachine.Execute();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("bullet"))
        {
            if (stateMachine.Target.Data != null)
            {
                HealthSystem.ChangeHealth(-stateMachine.Target.Data.Damage);
                stateMachine.ChangeState(stateMachine.GetHitState);
            }
        }
    }
    public void OnDamageCheck()
    {
        stateMachine.ChangeState(stateMachine.GetHitState);
    }
    public void OnDie()
    {
        Animator.SetTrigger("Die");
        gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, Data.PlayerChasingRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Data.AttackRange);
    }
    private void NavMeshSetting()
    {
        NvAgent.speed = Data.MoveSpeed;
        NvAgent.stoppingDistance = Data.AttackRange;
    }
}
