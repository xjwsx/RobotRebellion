using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpBar : MonoBehaviour
{
    public Transform player;
    public Slider hpSlider;
    public Text hpText;
    public float maxHp;
    public float currenHp;

    public GameObject hpLineFolder;
    float unitHp = 200f;

    public static PlayerHpBar instance;
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
    void Update()
    {
        transform.position = player.transform.position ;
        hpSlider.value = currenHp / maxHp;
        hpText.text = currenHp.ToString("N0");
    }

    public void GetHpBoost()
    {
        maxHp += 150;
        currenHp += 150;
        float scaleX = (1000f / unitHp) / (maxHp / unitHp);
        hpLineFolder.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(false);
        foreach(Transform child in hpLineFolder.transform)
        {
            child.gameObject.transform.localScale = new Vector3(scaleX, 1, 1);
        }
        hpLineFolder.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(true);
    }
}
