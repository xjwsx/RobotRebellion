using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject optionUI;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Button startButton;
    [SerializeField] private Button optionButton;
    [SerializeField] private Button exitButton;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        startButton.onClick.AddListener(OnLoadMainScene);
        optionButton.onClick.AddListener(OpenOptionUI);
    }
    public void OpenOptionUI()
    {
        audioSource.Play();
        StartCoroutine(nameof(ActivateOptionUI));
    }
    public void OnLoadMainScene()
    {
        audioSource.Play();
        LoadingSceneController.LoadScene("MainScene");
    }
    IEnumerator ActivateOptionUI()
    {
        var menuTask = UIManager.instance.GetUI<Option>();
        while (!menuTask.IsCompleted)
        {
            yield return null;
        }
    }
}
