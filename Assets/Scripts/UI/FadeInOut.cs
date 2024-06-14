using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    private Image fadeInOutImg;
    private void Awake()
    {
        fadeInOutImg = GetComponentInChildren<Image>();
    }
    private void OnEnable()
    {
        StartCoroutine(nameof(FadeInOutCo));
    }
    IEnumerator FadeInOutCo()
    {
        float a = 1;
        fadeInOutImg.color = new Vector4(1, 1, 1, a);
        yield return new WaitForSeconds(0.3f);

        while (a >= 0)
        {
            fadeInOutImg.color = new Vector4(1, 1, 1, a);
            a -= 0.02f;
            yield return null;
        }

    }
}
