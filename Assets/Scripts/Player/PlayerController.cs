using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    Rigidbody rb;
    public Animator animator;
    [SerializeField] private float moveSpeed = 5f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if(JoystickMovement.instance.joyVec.x != 0 || JoystickMovement.instance.joyVec.y != 0)
        {
            Vector3 moveDirection = JoystickMovement.instance.joyVec;
            rb.velocity = new Vector3(moveDirection.x * moveSpeed, rb.velocity.y, moveDirection.y * moveSpeed);
            rb.rotation = Quaternion.LookRotation(new Vector3(moveDirection.x,0, moveDirection.y));
        }
        else
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("NextStage"))
        {
            StageManager.instance.NextStage();
        }

        if(other.transform.CompareTag("HpBooster"))
        {
            PlayerHpBar.instance.GetHpBoost();
            Destroy(other.gameObject);
        }
        if (other.transform.CompareTag("MeleeAtk"))
        {
            other.transform.parent.GetComponent<EnemyController>().meleeAtkArea.SetActive(false);
            PlayerHpBar.instance.currenHp -= other.transform.parent.GetComponent<EnemyController>().damage * 2f;
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Dmg"))
            {
                animator.SetTrigger("Dmg");
                Instantiate(EffectSet.instance.playerDmgEffect, PlayerTargeting.instance.attackPoint.position, Quaternion.Euler(90, 0, 0));
            }
        }
    }
}
