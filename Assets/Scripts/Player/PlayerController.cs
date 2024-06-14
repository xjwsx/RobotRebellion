using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    private ObjectPoolManager pool;
    [SerializeField] private GameObject mainUI;
    private Slider expSlider;
    private TextMeshProUGUI expText;

    public List<SkillType> Skills { get; private set; } = new List<SkillType>();
    public float currentExp;
    public float maxExp;
    public int level = 1;
    public float criticalHitProbability = 0.1f;
    public float criticalHitMultiplier = 2.0f;

    private void Awake()
    {
        AnimationData.Initialize();
        pool = ObjectPoolManager.Instance;
        PlayerTargeting = GetComponent<PlayerTargeting>();
        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponent<Animator>();
        HealthSystem = GetComponent<HealthSystem>();
        HealthSystem.SetUp(Data.MaxHp);
        expSlider = mainUI.GetComponentInChildren<Slider>();
        expText = mainUI.GetComponentInChildren<TextMeshProUGUI>();
        SetUpExp(Data.Exp);
        stateMachine = new PlayerStateMachine(this);        
    }

    private void Start()
    {
        Animator.SetFloat("ShotSpeed", 0.7f);
        stateMachine.ChangeState(stateMachine.IdleingState);
        HealthSystem.OnDeath += OnDie;
    }
    private void Update()
    {
        stateMachine.Execute();
        UpdateEXPUI();
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
                GameObject obj = pool.SpawnFromPool("PlayerHitFX");
                Vector3 CurrentPostion = new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z);
                obj.transform.position = CurrentPostion;
                obj.SetActive(true);
            }
            else return;
        }
        if(other.gameObject.CompareTag("Angel"))
        {
            JoystickMovement.instance.Drop();
            StartCoroutine(nameof(ActivateSlotMachineUI));
            other.gameObject.SetActive(false);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("EXP"))
        {
            AddExp(10);
            collision.gameObject.SetActive(false);
            GameObject obj = pool.SpawnFromPool("PickupCoinFX");
            Vector3 CurrentPostion = new Vector3(collision.transform.position.x, collision.transform.position.y + 0.3f, collision.transform.position.z);
            obj.transform.position = CurrentPostion;
            obj.SetActive(true);
        }
    }
    IEnumerator ActivateSlotMachineUI()
    {
        var menuTask = UIManager.instance.GetUI<SlotMachine>();
        while (!menuTask.IsCompleted)
        {
            yield return null;
        }
    }
    public void AddExp(float amount)
    {
        currentExp += amount;
        if (currentExp >= maxExp)
        {
            LevelUPFx();
            StartCoroutine(LevelUp());
        }
    }
    IEnumerator LevelUp()
    {
        JoystickMovement.instance.Drop();
        yield return new WaitForSeconds(1f);
        currentExp = 0;
        maxExp += 50;
        expSlider.maxValue = maxExp;
        level++;
        JoystickMovement.instance.gameObject.SetActive(false);
        StartCoroutine(nameof(ActivateSlotMachineUI));
    }
    public void LevelUPFx()
    {
        GameObject obj = pool.SpawnFromPool("LevelUpFX");
        obj.transform.position = transform.position;
        obj.SetActive(true);
    }
    public void UpdateEXPUI()
    {
        expSlider.value = currentExp;
        expText.text = $"Lv.{level}";
    }
    public void AddSkill(SkillType skill)
    {
        if(!Skills.Contains(skill))
        {
            Skills.Add(skill);
            ActivateSkill(skill);
        }
    }
    public void ActivateSkill(SkillType skill)
    {
        switch (skill)
        {
            case SkillType.UpgradeBulletSpeed:
                IncreaseShotSpeed();
                break;
            case SkillType.UpgradeBulletDamage:
                IncreaseBulletDamage();
                break;
            case SkillType.UpgradeCrit:
                IncreaseCritDamage();
                break;
        }
    }
    private void IncreaseBulletDamage()
    {
        Data.Damage += 10;
    }
    private void IncreaseShotSpeed()
    {
        float currentSpeed = Animator.GetFloat("ShotSpeed");
        Animator.SetFloat("ShotSpeed", currentSpeed + 0.2f);
    }
    private void IncreaseCritDamage()
    {
        criticalHitProbability += 0.1f;
    }
    public void SetUpExp(int exp)
    {
        currentExp = 0;
        maxExp = exp;
        expSlider.maxValue = maxExp;
        expSlider.value = 0;
    }
    public void OnDie()
    {
        Animator.SetTrigger("Die");
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Data.AttackRange);
    }
}
