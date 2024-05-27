using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        StartCoroutine(LoadSceneProcess());
    }

    IEnumerator LoadSceneProcess()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float timer = 0f;
        while (!op.isDone)
        {
            yield return null;

            if(op.progress < 0.9f)
            {
                progressBar.fillAmount = op.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                progressBar.fillAmount = Mathf.Lerp(0f, 1f, timer);
                if(progressBar.fillAmount >= 1f)
                {
                    //SetImageTransparency(progressBar, 0f);
                    //SetImageTransparency(progressBarImg, 0f);
                    textUI.gameObject.SetActive(true);

                    yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }

    private void SetImageTransparency(Image img, float alpha)
    {
        if(img != null)
        {
            Color color = img.color;
            color.a = alpha;
            img.color = color;
        }
    }
}
