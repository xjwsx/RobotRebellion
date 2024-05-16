using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    private GameObject walkSoundObject;
    private ObjectPoolManager pool;

    [SerializeField] private AudioClip bgmStart;
    [SerializeField] private AudioClip bgmInGame;
    [SerializeField] private AudioClip buttonClick;
    //[SerializeField] private AudioClip walk;
    //[SerializeField] private AudioClip attack;

    private AudioSource BGM;
    public event Action<bool> OnButtonClick;
    private bool isButtonClick = false;

    public bool IsButtonClick
    {
        get { return isButtonClick; }
        set
        {
            isButtonClick = value;
            OnButtonClick?.Invoke(value);
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        pool = GetComponent<ObjectPoolManager>();
        BGM = GetComponent<AudioSource>();
        BGM.volume = 0.5f;
        BGM.loop = true;
    }
    private void Start()
    {
        PlayGameBgm(bgmStart);
        SceneManager.sceneLoaded += CheckStartScene;
        OnButtonClick += PlayButtonClickSound;
    }
    public void CheckStartScene(Scene changed, LoadSceneMode loadSceneMode)
    {
        if (changed.buildIndex == 2)
        {
            PlayGameBgm(bgmStart);
        }
        else if(changed.buildIndex == 3)
        {
            PlayGameBgm(bgmInGame);
            pool.InitializePool();
        }
    }
    public void PlayGameBgm(AudioClip bgm)
    {
        BGM.Stop();
        BGM.clip = bgm;
        BGM.Play();
    }
    public void PlayClip(AudioClip clip)
    {
        GameObject obj = pool.SpawnFromPool("SoundSource");
        obj.SetActive(true);
        SoundSource soundSource = obj.GetComponent<SoundSource>();
        soundSource.Play(clip);
    }
    public void PlayWalkClip(AudioClip clip)
    {
        if (walkSoundObject == null)
        {
            walkSoundObject = pool.SpawnFromPool("WalkSoundSource");
        }
        walkSoundObject.SetActive(true);
        SoundSource soundSource = walkSoundObject.GetComponent<SoundSource>();
        soundSource.PlayWalk(clip);
    }
    public void StopWalkClip()
    {
        if (walkSoundObject != null)
        {
            SoundSource soundSource = walkSoundObject.GetComponent<SoundSource>();
            soundSource.Stop();
        }
    }
    public void PlayButtonClickSound(bool isOn)
    {
        if (isOn)
        {
            PlayClip(buttonClick);
        }
    }
}
