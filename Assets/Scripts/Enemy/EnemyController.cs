using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private ObjectPoolManager pool;
    private EnemyStateMachine stateMachine;

    [field: Header("References")]
    [field: SerializeField] public EnemySO Data { get; private set; }

    [field: Header("Animations")]
    [field: SerializeField] public EnemyAnimationData AnimationData { get; private set; }
    public Rigidbody Rigidbody { get; private set; }
    public Animator Animator { get; private set; }
    public HealthSystem HealthSystem { get; private set; }
    public NavMeshAgent NvAgent { get; private set; }

    public Transform attackPoint;
    public float bulletSpeed = 10f;
    public float drawTime = 1f;
    private void Awake()
    {
        pool = ObjectPoolManager.Instance;
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
            }
        }
    }
    public void OnDie()
    {
        Animator.SetTrigger("Die");
        gameObject.SetActive(false);
        stateMachine.Target.PlayerTargeting.MonsterList.Remove(gameObject);
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
    public void Attack()
    {
        GameObject obj = pool.SpawnFromPool("monsterBullet");
        obj.SetActive(true);
        Shoot(obj);
    }
    public void Shoot(GameObject obj, float angle = 0f)
    {
        obj.transform.SetPositionAndRotation(attackPoint.position, Quaternion.Euler(0, angle, 0));
        obj.GetComponent<Rigidbody>().velocity = Quaternion.Euler(0, angle, 0) * transform.forward * bulletSpeed;
    }
    
}
