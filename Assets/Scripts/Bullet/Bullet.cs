using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int maxBounces = 3;
    private int bounceCount = 0;
    private void OnEnable()
    {
        bounceCount = 0;
        if(!GameManager.instance.playerController.Skills.Contains(SkillType.BoundWallBullet))
        {
            StopAllCoroutines();
            StartCoroutine(nameof(DeactivationTimer));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Monster"))
        {
            gameObject.SetActive(false);
        }        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Stage") && 
            GameManager.instance.playerController.Skills.Contains(SkillType.BoundWallBullet) && 
            bounceCount < maxBounces)
        {
            bounceCount++;
            Bounce(collision);
        }
    }
    private void Bounce(Collision wall)
    {
        Vector3 incomingVector = transform.forward;
        Vector3 normalVector = wall.contacts[0].normal;
        Vector3 reflectVector = Vector3.Reflect(incomingVector, normalVector);
        transform.forward = reflectVector;
        if (bounceCount >= maxBounces)
            gameObject.SetActive(false);
    }
    private IEnumerator DeactivationTimer()
    {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }
}
