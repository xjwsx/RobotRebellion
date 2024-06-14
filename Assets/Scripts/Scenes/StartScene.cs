using System.Collections;
using UnityEngine;

public class StartScene : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(nameof(ActivateMenuUI));
    }
    IEnumerator ActivateMenuUI()
    {
        var menuTask = UIManager.instance.GetUI<Menu>();
        while (!menuTask.IsCompleted)
        {
            yield return null;
        }
    }
}
