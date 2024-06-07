using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneController : MonoBehaviour
{
    static string nextScene;

    [SerializeField] private Image progressBar;
    [SerializeField] private Image progressBarImg;
    [SerializeField] private TextMeshProUGUI textUI;

    public static void LoadScene(string name)
    {
        nextScene = name;

        SceneManager.LoadScene("LoadingScene");
    }
    private async void Start()
    {
        await LoadSceneAsync(nextScene);
    }

    private async Task LoadSceneAsync(string sceneName)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;
        while(!op.isDone)
        {
            if(op.progress < 0.9f)
            {
                progressBar.fillAmount = op.progress;
            }
            else
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, Time.unscaledDeltaTime);
                if (progressBar.fillAmount >= 0.99f)
                {
                    textUI.gameObject.SetActive(true);
                    await WaitForMouseClick();
                    op.allowSceneActivation = true;
                    return;
                }
            }
            await Task.Yield();
        }
    }

    private Task WaitForMouseClick()
    {
        var tcs = new TaskCompletionSource<bool>();
        StartCoroutine(WaitForClickCoroutine(tcs));
        return tcs.Task;
    }
    private IEnumerator WaitForClickCoroutine(TaskCompletionSource<bool> tcs)
    {
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        tcs.SetResult(true);
    }
}
