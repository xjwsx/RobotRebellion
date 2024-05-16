using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private GameObject InventoryUI;
    public PlayerController playerController;
    public EnemyController enemyController;
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

    public void PauseButton()
    {
        AudioManager.instance.IsButtonClick = true;
        Time.timeScale = 0f;
        InventoryUI.SetActive(true);
    }
    public void PlayButton()
    {
        AudioManager.instance.IsButtonClick = true;
        Time.timeScale = 1f;
        InventoryUI.SetActive(false);
    }
}
