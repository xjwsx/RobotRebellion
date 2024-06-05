using UnityEngine;

public class GameManager : Singleton<GameManager>   
{
    public GameObject joystickCanvasUI;
    public PauseUI PauseUI;
    public GameObject slotMachineUI;
    public PlayerController playerController;
    public EnemyController enemyController;

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
