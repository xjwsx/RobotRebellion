using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : MonoBehaviour
{
    [SerializeField] private GameObject optionUI;
    [SerializeField] private AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OpenOptionUI()
    {
        audioSource.Play();
        optionUI.SetActive(true);
    }
    public void OnLoadMainScene()
    {
        audioSource.Play();
        LoadingSceneController.LoadScene("MainScene");
    }
    public void CloseButton()
    {
        audioSource.Play();
        optionUI.SetActive(false);
    }
}
