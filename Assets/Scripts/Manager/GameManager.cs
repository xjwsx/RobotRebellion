using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private GameObject InventoryUI;
    [SerializeField] private GameObject joystickUI;
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
        joystickUI.SetActive(false);
    }
    public void PlayButton()
    {
        AudioManager.instance.IsButtonClick = true;
        Time.timeScale = 1f;
        InventoryUI.SetActive(false);
        joystickUI.SetActive(true);
    }
}
