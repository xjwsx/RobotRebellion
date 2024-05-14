using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCondition : MonoBehaviour
{
    List<GameObject> MonsterListInRoom = new();
    public bool playerInThisRoom = false;
    public bool isClearRoom = false;

    void Update()
    {
        if(playerInThisRoom)
        {
            if(MonsterListInRoom.Count <= 0 && !isClearRoom)
            {
                isClearRoom = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerInThisRoom = true;
            GameManager.instance.playerController.PlayerTargeting.MonsterList = new List<GameObject>(MonsterListInRoom);
        }
        if(other.CompareTag("Monster"))
        {
            MonsterListInRoom.Add(other.transform.parent.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInThisRoom = false;
            GameManager.instance.playerController.PlayerTargeting.MonsterList.Clear();
        }
        if (other.CompareTag("Monster"))
        {
            MonsterListInRoom.Remove(other.transform.parent.gameObject);
        }
    }
}
