using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomCondition : MonoBehaviour
{
    public List<GameObject> MonsterListInRoom = new();
    public bool playerInThisRoom = false;
    public bool isClearRoom = false;
    private GameObject openDoor;
    private GameObject closeDoor;
    private void Start()
    {
        openDoor = transform.GetChild(6).gameObject;
        closeDoor = transform.GetChild(5).gameObject;
    }
    private void Update()
    {
        if(playerInThisRoom)
        {
            CheckRoomClearance();
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
            MonsterListInRoom.Add(other.transform.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInThisRoom = false;
            isClearRoom = false;
            GameManager.instance.playerController.PlayerTargeting.MonsterList.Clear();
        }
    }

    private void CheckRoomClearance()
    {
        if (!isClearRoom && GameManager.instance.playerController.PlayerTargeting.MonsterList.Count == 0)
        {
            isClearRoom = true;
            openDoor.SetActive(true);
            closeDoor.SetActive(false);
        }
    }
}
