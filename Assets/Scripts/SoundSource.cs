using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSource : MonoBehaviour
{
    public AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void Play(AudioClip clip)
    {
        if(audioSource == null)
            audioSource = GetComponent<AudioSource>();

        CancelInvoke();
        audioSource.clip = clip;
        audioSource.volume = 1f;
        audioSource.Play();

        Invoke(nameof(Disable), clip.length + 1);
    }

    public void Disable()
    {
        audioSource.Stop();
        gameObject.SetActive(false);
    }
    public void PlayWalk(AudioClip clip)
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = 0.8f;
        audioSource.pitch = 1.8f;
        audioSource.loop = true;
        audioSource.Play();
    }
    public void Stop()
    {
        audioSource.loop = false;
        audioSource.Stop();
        gameObject.SetActive(false);
    }
}
