using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : MonoBehaviour
{
    [SerializeField] private GameObject optionUI;

    public void OpenOptionUI()
    {
        AudioManager.instance.IsButtonClick = true;
        optionUI.SetActive(true);
    }
    public void OnLoadMainScene()
    {
        AudioManager.instance.IsButtonClick = true;
        LoadingSceneController.LoadScene("MainScene");
    }
    public void CloseButton()
    {
        AudioManager.instance.IsButtonClick = true;
        optionUI.SetActive(false);
    }
}
