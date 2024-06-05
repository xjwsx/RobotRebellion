using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomCondition : Singleton<RoomCondition>
{
    public List<GameObject> MonsterListInRoom = new();
    public bool playerInThisRoom = false;
    public bool isClearRoom = false;

    public event Action<bool> OnClearRoom;

    public bool IsClearRoom
    {
        get { return isClearRoom; }
        set
        {
            if(isClearRoom != value)
            {
                isClearRoom = value;
                if(isClearRoom)
                {
                    OnClearRoom?.Invoke(value);
                }
            }

        }
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
        if (!IsClearRoom && GameManager.instance.playerController.PlayerTargeting.MonsterList.Count == 0)
        {
            IsClearRoom = true;

            StageManager.instance.OpenDoor();
        }
    }
}
