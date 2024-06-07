using System.Collections;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    private EnemyController enemyController;
    private PlayerController target;
    public LayerMask layerMask;
    public bool lookAtPlayer = true;
    public LineRenderer lineRenderer;
    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        enemyController = GetComponent<EnemyController>();
    }
    private void Start()
    {
        StartCoroutine(nameof(WaitForPlayer));
    }
    IEnumerator WaitForPlayer()
    {
        yield return null;

        //while (!RoomCondition.instance.playerInThisRoom)
        //{
        //    yield return new WaitForSeconds(0.5f);
        //}

        yield return new WaitForSeconds(0.5f);
        StartCoroutine(AimAtPlayer());

        yield return new WaitForSeconds(2f);
        DeactivateDangerMarker();
        enemyController.Attack();
    }

    public IEnumerator AimAtPlayer()
    {
        while (lookAtPlayer)
        {
            yield return null;
            if (target != null)
            {
                transform.LookAt(target.transform.position);
                ShowDangerMarker();
            }
        }
    }

    public void ShowDangerMarker()
    {
        Vector3 NewPosition = enemyController.attackPoint.position;
        Vector3 NewDir = transform.forward;
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, transform.position);
        for (int i = 1; i < 4; i++)
        {
            if(Physics.Raycast(NewPosition, NewDir, out RaycastHit hit, 30f, layerMask))
            {
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(i, hit.point);

                NewPosition = hit.point;
                NewDir = Vector3.Reflect(NewDir, hit.normal);
            }
            else
            {
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(i, NewPosition + NewDir * 30f);
                break;
            }

        }
        
    }

    public void DeactivateDangerMarker()
    {
        lookAtPlayer = false;
        lineRenderer.positionCount = 0;
    }
}
