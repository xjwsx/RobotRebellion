using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : EnemyMeleeFSM
{
    public GameObject enemyCanvasGo;
    public GameObject meleeAtkArea;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, playerRealizeRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private void Start()
    {
        base.Start();
        attackCoolTime = 2f;
        attackCoolTimeCacl = attackCoolTime;

        attackRange = 3f;
        nvAgent.stoppingDistance = 1f;

        StartCoroutine(ResetAtkArea());
    }

    IEnumerator ResetAtkArea()
    {
        while (true)
        {
            yield return null;
            if (!meleeAtkArea.activeInHierarchy && currentState == State.Attack)
            {
                yield return new WaitForSeconds(attackCoolTime);
                meleeAtkArea.SetActive(true);
            }
        }
    }

    protected override void InitMonster()
    {
        maxHp += (StageManager.instance.currentStage + 1) * 100f;
        currentHp = maxHp;
        damage += (StageManager.instance.currentStage + 1) * 10f;
    }

    protected override void AtkEffect()
    {
        Instantiate(EffectSet.instance.monsterAtkEffect, transform.position, Quaternion.Euler(90, 0, 0));
    }

    void Update()
    {
        if (currentHp <= 0)
        //if ( enemyCanvasGo.GetComponent<EnemyHpBar> ( ).currentHp <= 0 )
        {
            nvAgent.isStopped = true;

            rb.gameObject.SetActive(false);
            PlayerTargeting.instance.MonsterList.Remove(transform.parent.gameObject);
            PlayerTargeting.instance.targetIndex = -1;
            Destroy(transform.parent.gameObject);
            return;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("bullet"))
        {
            enemyCanvasGo.GetComponent<EnemyHpBar>().Dmg();
            currentHp -= 250f;
            Instantiate(EffectSet.instance.monsterDmgEffect, collision.contacts[0].point, Quaternion.Euler(90, 0, 0));
        }
    }
}
