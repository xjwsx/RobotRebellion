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
    public LayerMask layerMask;
    public LineRenderer lineRenderer;
    public bool lookAtPlayer = true;
    public bool isPreparingAttack = false;
    private bool isDangerMarkerActive = false;
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
        HealthSystem.OnDeath += Coin;
        HealthSystem.OnDeath += EnemyDeathFx;
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
                GameObject obj = pool.SpawnFromPool("EnemyHitFX");
                Vector3 CurrentPostion = new Vector3(other.transform.position.x, other.transform.position.y + 0.3f, other.transform.position.z);
                obj.transform.position = CurrentPostion;
                obj.SetActive(true);
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
    public void Coin()
    {
        Vector3 CurrentPostion = new Vector3(transform.position.x, 0.7f, transform.position.z);
        for (int i = 0; i < (StageManager.instance.currentStage / 10 + 2 + Random.Range(0, 3)); i++)
        {
            GameObject obj = pool.SpawnFromPool("EXP");
            obj.transform.position = CurrentPostion;
            obj.SetActive(true);
        }
    }
    public void EnemyDeathFx()
    {
        GameObject obj = pool.SpawnFromPool("EnemyDeathFX");
        Vector3 CurrentPostion = new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z);
        obj.transform.position = CurrentPostion;
        obj.SetActive(true);
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
    public IEnumerator WaitForPlayer()
    {
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(AimAtPlayer());

        yield return new WaitForSeconds(2f);
        DeactivateDangerMarker();
    }
    public IEnumerator AimAtPlayer()
    {
        lookAtPlayer = true;
        while (lookAtPlayer && stateMachine.Target != null)
        {
            yield return null;
            if (!isDangerMarkerActive)
            {
                transform.LookAt(stateMachine.Target.transform.position);
                ShowDangerMarker();
            }
        }
        isDangerMarkerActive = false;
    }
    public void ShowDangerMarker()
    {
        Vector3 NewPosition = attackPoint.position;
        Vector3 NewDir = transform.forward;
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, transform.position);

        for (int i = 0; i < 2; i++)
        {
            Physics.Raycast(NewPosition, NewDir, out RaycastHit hit, 30f, layerMask);

            lineRenderer.positionCount++;
            lineRenderer.SetPosition(i, hit.point);

            NewPosition = hit.point;
            NewDir = Vector3.Reflect(NewDir, hit.normal);
        }
    }
    public void DeactivateDangerMarker()
    {
        lookAtPlayer = false;
        lineRenderer.positionCount = 0;
        stateMachine.ChangeState(stateMachine.AttackState);
    }
}
