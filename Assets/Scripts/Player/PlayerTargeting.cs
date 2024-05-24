using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargeting : MonoBehaviour
{
    private ObjectPoolManager pool;
    private PlayerController controller;
    public bool getATarget = false;
    public int targetIndex = -1; //타겟팅 할 인덱스
    //private int closeDistIndex = 0; //가장 가까운 인덱스
    //private int prevTargetIndex = 0; 
    //private float currentDist = 0; // 현재거리
    //private float closetDist = 100f; //가까운 거리
    //private float targetDist = 100f; //타겟 거리

    public LayerMask layerMask;
    public List<GameObject> MonsterList = new List<GameObject>();
    public float bulletSpeed = 10f;
    public Transform attackPoint;

    private void Awake()
    {
        pool = ObjectPoolManager.Instance;
        controller = GetComponent<PlayerController>();
    }

    private void OnDrawGizmos()
    {
        if( getATarget )
        {
            for (int i = 0; i < MonsterList.Count; i++)
            {
                if (MonsterList[i] == null) { return; }
                RaycastHit hit;
                bool isHit = Physics.Raycast(transform.position, MonsterList[i].transform.GetChild(0).position - transform.position
                    , out hit, 20f, layerMask);
                Gizmos.color = isHit && hit.transform.CompareTag("Monster") ? Color.green : Color.red;
                Gizmos.DrawRay(transform.position, MonsterList[i].transform.GetChild(0).position - transform.position);
            }
        }
    }
    private void Update()
    {
        SetTarget();
        AtkTarget();
    }

    public void Attack()
    {
        GameObject obj = pool.SpawnFromPool("bullet");
        obj.SetActive(true);
        Shoot(obj);
    }
    public void Shoot(GameObject obj, float angle = 0f)
    {
        obj.transform.SetPositionAndRotation(attackPoint.position, Quaternion.Euler(0, angle, 0));
        obj.GetComponent<Rigidbody>().velocity = Quaternion.Euler(0, angle, 0) * transform.forward * bulletSpeed;
    }
    public void SetTarget()
    {
        if (MonsterList.Count == 0) return;

        targetIndex = -1;
        float closestDistance = Mathf.Infinity;

        for (int i = 0; i < MonsterList.Count; i++)
        {
            if (MonsterList[i] == null) continue;

            float distance = Vector3.Distance(transform.position, MonsterList[i].transform.GetChild(0).position);
            RaycastHit hit;
            bool isHit = Physics.Raycast(transform.position, MonsterList[i].transform.GetChild(0).position - transform.position, out hit, controller.Data.AttackRange, layerMask);

            if (isHit && hit.transform.CompareTag("Monster") && distance < closestDistance)
            {
                closestDistance = distance;
                targetIndex = i;
            }
        }

        getATarget = targetIndex != -1;
    }
    public void AtkTarget()
    {
        if (targetIndex == -1 || targetIndex >= MonsterList.Count) return;

        var targetMonster = MonsterList[targetIndex];
        if (targetMonster == null || !targetMonster.activeSelf)
        {
            targetIndex = -1;
            getATarget = false;
            return;
        }

        float distanceToTarget = Vector3.Distance(transform.position, targetMonster.transform.GetChild(0).position);

        if (distanceToTarget <= controller.Data.AttackRange)
        {
            if (getATarget && !JoystickMovement.instance.isPlayerMoving)
            {
                transform.LookAt(targetMonster.transform.GetChild(0));
            }
        }
    }
    //public void SetTarget()
    //{
    //    if (MonsterList.Count != 0)
    //    {
    //        prevTargetIndex = targetIndex;
    //        currentDist = 0f;
    //        closeDistIndex = 0;
    //        targetIndex = -1;

    //        for(int i = 0; i < MonsterList.Count; i++)
    //        {
    //            if (MonsterList[i] == null) { return; }
    //            currentDist = Vector3.Distance(transform.position, MonsterList[i].transform.GetChild(0).position);
    //            RaycastHit hit;
    //            bool isHit = Physics.Raycast(transform.position, MonsterList[i].transform.GetChild(0).position - transform.position
    //                , out hit, controller.Data.AttackRange, layerMask);
    //            if (isHit && hit.transform.CompareTag("Monster"))
    //            {
    //                if(targetDist >= currentDist)
    //                {
    //                    targetIndex = i;
    //                    targetDist = currentDist;
    //                    if(!JoystickMovement.instance.isPlayerMoving && prevTargetIndex != targetIndex)
    //                    {
    //                        targetIndex = prevTargetIndex;
    //                    }
    //                }
    //            }
    //            if(closetDist >= currentDist)
    //            {
    //                closeDistIndex = i;
    //                closetDist = currentDist;
    //            }
    //        }
    //        if(targetIndex == -1)
    //        {
    //            targetIndex = closeDistIndex;                
    //        }
    //        closetDist = 100f;
    //        targetDist = 100f;
    //        getATarget = true;
    //    }
    //}
    //public void AtkTarget()
    //{
    //    if (targetIndex >= 0 && targetIndex < MonsterList.Count)
    //    {
    //        var targetMonster = MonsterList[targetIndex];
    //        if (targetMonster != null && targetMonster.activeSelf)
    //        {
    //            float distanceToTarget = Vector3.Distance(transform.position, targetMonster.transform.GetChild(0).position);

    //            if (distanceToTarget <= controller.Data.AttackRange)
    //            {
    //                if (getATarget && !JoystickMovement.instance.isPlayerMoving)
    //                {
    //                    transform.LookAt(targetMonster.transform.GetChild(0));
    //                }
    //            }
    //        }
    //        else
    //        {
    //            targetIndex = -1;
    //            getATarget = false;
    //        }
    //    }
    //    else
    //    {
    //        targetIndex = -1;
    //        getATarget = false;
    //    }
    //}

    //public void AtkTarget()
    //{
    //    float distanceToTarget = Vector3.Distance(transform.position, MonsterList[targetIndex].transform.GetChild(0).position);

    //    if (distanceToTarget <= maxAttackDistance)
    //    {
    //        if (getATarget && !JoystickMovement.instance.isPlayerMoving && MonsterList.Count != 0 && MonsterList[targetIndex].activeSelf)
    //        {
    //            transform.LookAt(MonsterList[targetIndex].transform.GetChild(0));
    //        }
    //    }
    //}
    public void BounceAttack(GameObject hitMonster)
    {
        float bounceRadius = 5f;
        Collider[] hitColliders = Physics.OverlapSphere(hitMonster.transform.position, bounceRadius);

        foreach (var collider in hitColliders)
        {
            if (collider.CompareTag("Monster") && collider.gameObject != hitMonster)
            {
                var healthSystem = collider.GetComponent<HealthSystem>();
                if (healthSystem != null)
                {
                    healthSystem.ChangeHealth(-10);
                }

                Rigidbody rb = collider.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Vector3 bounceDirection = (collider.transform.position - hitMonster.transform.position).normalized;
                    rb.AddForce(bounceDirection * 5f, ForceMode.Impulse);
                }
            }
        }
    }

    public void DoubleBulletAttack()
    {
        GameObject bullet1 = pool.SpawnFromPool("bullet");
        GameObject bullet2 = pool.SpawnFromPool("bullet");

        bullet1.SetActive(true);
        bullet2.SetActive(true);

        Shoot(bullet1);
        Shoot(bullet2);
    }

    public void TripleBulletAttack()
    {
        GameObject bullet1 = pool.SpawnFromPool("bullet");
        GameObject bullet2 = pool.SpawnFromPool("bullet");
        GameObject bullet3 = pool.SpawnFromPool("bullet");

        bullet1.SetActive(true);
        bullet2.SetActive(true);
        bullet3.SetActive(true);

        Shoot(bullet1);
        Shoot(bullet2, 45); 
        Shoot(bullet3, -45);
    }
}
