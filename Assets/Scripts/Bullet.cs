using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Monster") || other.gameObject.CompareTag("Stage"))
        {
            gameObject.SetActive(false);
        }
        
    }
}
