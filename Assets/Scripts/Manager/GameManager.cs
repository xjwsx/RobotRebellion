using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour   
{
    public static GameManager instance;
    public GameObject joystickCanvasUI;
    public PauseUI PauseUI;
    public GameObject slotMachineUI;
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
        //AudioManager.instance.IsButtonClick = true;
        Time.timeScale = 0f;
        PauseUI.gameObject.SetActive(true);
        PauseUI.ActiveButtons();
        joystickCanvasUI.SetActive(false);
    }
    public void PlayButton()
    {
        //AudioManager.instance.IsButtonClick = true;
        Time.timeScale = 1f;
        PauseUI.gameObject.SetActive(false);
        joystickCanvasUI.SetActive(true);
    }
    public void SlotMachineOn()
    {
        //AudioManager.instance.IsButtonClick = true;
        slotMachineUI.SetActive(true);
        joystickCanvasUI.SetActive(false);
    }
}
