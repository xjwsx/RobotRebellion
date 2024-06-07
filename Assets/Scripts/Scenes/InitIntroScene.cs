using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class InitIntroScene : MonoBehaviour
{
    [SerializeField] private PlayableDirector timeLineDirector;
    [SerializeField] private GameObject uiIntro;

    private void Start()
    {
        PlayIntro();
    }

    private void PlayIntro()
    {
        timeLineDirector.Play();
        uiIntro.SetActive(true);
        StartCoroutine(OnTimelineFinishedCoroutine());
    }
    private IEnumerator OnTimelineFinishedCoroutine()
    {
        yield return new WaitForSeconds((float)timeLineDirector.duration);
        OnTimelineFinished();
    }
    private void OnTimelineFinished()
    {
        LoadingSceneController.LoadScene("StartScene");
    }
}
