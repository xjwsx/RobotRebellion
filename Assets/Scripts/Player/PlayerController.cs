using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [field: Header("References")]
    [field: SerializeField] public PlayerSO Data { get; private set; }

    [field: Header("Animations")]
    [field: SerializeField] public PlayerAnimationData AnimationData { get; private set; }
    public PlayerTargeting PlayerTargeting { get; private set; }
    public Rigidbody Rigidbody { get; private set; }
    public Animator Animator { get; private set; }
    public PlayerHpBar PlayerHpBar { get; private set; }
    public HealthSystem HealthSystem { get; private set; }
    private PlayerStateMachine stateMachine;

    public List<SkillType> Skills { get; private set; } = new List<SkillType>();

    private void Awake()
    {
        AnimationData.Initialize();

        PlayerTargeting = GetComponent<PlayerTargeting>();
        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponent<Animator>();
        HealthSystem = GetComponent<HealthSystem>();
        HealthSystem.SetUp(Data.MaxHp);
        stateMachine = new PlayerStateMachine(this);        
    }

    private void Start()
    {
        stateMachine.ChangeState(stateMachine.IdleingState);
        HealthSystem.OnDeath += OnDie;
        //HealthSystem.OnDeath += MonsterCheck;
    }
    private void Update()
    {
        stateMachine.Execute();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("NextStage"))
        {
            StageManager.instance.NextStage();
        }
        if (other.gameObject.CompareTag("Monster") || other.gameObject.CompareTag("MonsterBullet"))
        {
            if (stateMachine.Target.Data != null)
            {
                HealthSystem.ChangeHealth(-stateMachine.Target.Data.Damage);
            }
        }
    }
    public void AddSkill(SkillType skill)
    {
        Skills.Add(skill);
    }

    public void OnDie()
    {
        Animator.SetTrigger("Die");
    }
    //public void MonsterCheck()
    //{
    //    for (int i = 0; i < PlayerTargeting.MonsterList.Count; i++)
    //    {
    //        if (!PlayerTargeting.MonsterList[i].activeSelf)
    //        {
    //            PlayerTargeting.MonsterList.RemoveAt(i);
    //        }
    //    }
    //}
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Data.AttackRange);
    }
}
