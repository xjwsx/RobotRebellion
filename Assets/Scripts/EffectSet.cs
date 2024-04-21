using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSet : MonoBehaviour
{
    public static EffectSet instance;
    [Header("Monster")]
    public GameObject monsterAtkEffect;
    public GameObject monsterDmgEffect;

    [Header("Player")]
    public GameObject playerAtkEffect;
    public GameObject playerDmgEffect;

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
    }
}
