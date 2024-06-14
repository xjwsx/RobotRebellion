using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    [SerializeField] private Button pauseButton;

    private void Start()
    {
        pauseButton.onClick.AddListener(PauseButton);
    }
    public void PauseButton()
    {
        //AudioManager.instance.IsButtonClick = true;
        Time.timeScale = 0f;
        StartCoroutine(nameof(ActivatePauseUI));
        JoystickMovement.instance.gameObject.SetActive(false);
    }
    IEnumerator ActivatePauseUI()
    {
        var menuTask = UIManager.instance.GetUI<Pause>();
        while (!menuTask.IsCompleted)
        {
            yield return null;
        }
    }
}
