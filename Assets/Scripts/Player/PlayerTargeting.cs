using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargeting : MonoBehaviour
{
    public static PlayerTargeting instance;
    private ObjectPoolManager pool;

    public bool getATarget = false;
    private int closeDistIndex = 0; //가장 가까운 인덱스
    private int prevTargetIndex = 0; 
    public int targetIndex = -1; //타겟팅 할 인덱스
    private float currentDist = 0; // 현재거리
    private float closetDist = 100f; //가까운 거리
    private float targetDist = 100f; //타겟 거리
    public float atkSpd = 1f;

    public LayerMask layerMask;
    public List<GameObject> MonsterList = new List<GameObject>();
    public float bulletSpeed = 10f;
    public Transform attackPoint;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            pool = ObjectPoolManager.instance;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
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

                if(isHit && hit.transform.CompareTag("Monster"))
                {
                    Gizmos.color = Color.green;
                }
                else
                {
                    Gizmos.color = Color.red;
                }
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
        PlayerController.instance.animator.SetBool("Shot", true);
        GameObject obj = pool.SqawnFromPool("bullet");
        obj.SetActive(true);
        Shoot(obj);
    }
    public void Shoot(GameObject obj)
    {
        obj.transform.position = attackPoint.position;
        obj.transform.rotation = Quaternion.identity;
        obj.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;
    }
    public void SetTarget()
    {
        if(MonsterList.Count != 0)
        {
            prevTargetIndex = targetIndex;
            currentDist = 0f;
            closeDistIndex = 0;
            targetIndex = -1;

            for(int i = 0; i < MonsterList.Count; i++)
            {
                if (MonsterList[i] == null) { return; }
                currentDist = Vector3.Distance(transform.position, MonsterList[i].transform.GetChild(0).position);
                RaycastHit hit;
                bool isHit = Physics.Raycast(transform.position, MonsterList[i].transform.GetChild(0).position - transform.position
                    , out hit, 20f, layerMask);
                if(isHit && hit.transform.CompareTag("Monster"))
                {
                    if(targetDist >= currentDist)
                    {
                        targetIndex = i;
                        targetDist = currentDist;
                        if(!JoystickMovement.instance.isPlayerMoving && prevTargetIndex != targetIndex)
                        {
                            targetIndex = prevTargetIndex;
                        }
                    }
                }
                if(closetDist >= currentDist)
                {
                    closeDistIndex = i;
                    closetDist = currentDist;
                }
            }
            if(targetIndex == -1)
            {
                targetIndex = closeDistIndex;
            }
            closetDist = 100f;
            targetDist = 100f;
            getATarget = true;
        }
    }

    public void AtkTarget()
    {
        if(targetIndex == -1 || MonsterList.Count ==0)
        {
            PlayerController.instance.animator.SetBool("Shot", false);
            return;
        }
        if(getATarget && !JoystickMovement.instance.isPlayerMoving && MonsterList.Count != 0)
        {
            transform.LookAt(MonsterList[targetIndex].transform.GetChild(0));
            //Attack();
            PlayerController.instance.animator.SetBool("Shot", true) ;
            PlayerController.instance.animator.SetBool("Run", false);
        }
        else if(JoystickMovement.instance.isPlayerMoving)
        {
            PlayerController.instance.animator.SetBool("Shot", false);
            PlayerController.instance.animator.SetBool("Run", true);
        }
        else
        {
            PlayerController.instance.animator.SetBool("Shot", false);
            PlayerController.instance.animator.SetBool("Run", false);
        }
    }
}
