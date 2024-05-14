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

    //private void Start()
    //{
    //    base.Start();
    //    attackCoolTime = 2f;
    //    attackCoolTimeCacl = attackCoolTime;

    //    attackRange = 3f;
    //    nvAgent.stoppingDistance = 1f;

    //    StartCoroutine(ResetAtkArea());
    //}

    //IEnumerator ResetAtkArea()
    //{
    //    while (true)
    //    {
    //        yield return null;
    //        if (!meleeAtkArea.activeInHierarchy && currentState == State.Attack)
    //        {
    //            yield return new WaitForSeconds(attackCoolTime);
    //            meleeAtkArea.SetActive(true);
    //        }
    //    }
    //}

    //protected override void InitMonster()
    //{
    //    maxHp += (StageManager.instance.currentStage + 1) * 100f;
    //    currentHp = maxHp;
    //    damage += (StageManager.instance.currentStage + 1) * 10f;
    //}

    //protected override void AtkEffect()
    //{
    //    Instantiate(EffectSet.instance.monsterAtkEffect, transform.position, Quaternion.Euler(90, 0, 0));
    //}

    //void Update()
    //{
    //    if (currentHp <= 0)
    //    //if ( enemyCanvasGo.GetComponent<EnemyHpBar> ( ).currentHp <= 0 )
    //    {
    //        nvAgent.isStopped = true;

    //        rb.gameObject.SetActive(false);
    //        PlayerTargeting.instance.MonsterList.Remove(transform.parent.gameObject);
    //        PlayerTargeting.instance.targetIndex = -1;
    //        Destroy(transform.parent.gameObject);
    //        return;
    //    }
    //}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.transform.CompareTag("bullet"))
    //    {
    //        enemyCanvasGo.GetComponent<EnemyHpBar>().Dmg();
    //        currentHp -= 250f;
    //        Instantiate(EffectSet.instance.monsterDmgEffect, collision.contacts[0].point, Quaternion.Euler(90, 0, 0));
    //    }
    //}
}
