using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpBar : MonoBehaviour
{
    public Transform enemy;
    public Slider hpSlider;
    public Slider backHpSlider;
    public Text hpText;

    public bool backHpHit = false;
    public float maxHp;
    public float currenHp;
    void Update()
    {
        transform.position = enemy.transform.position;
        hpSlider.value = Mathf.Lerp(hpSlider.value,currenHp / maxHp, Time.deltaTime * 5f);
        hpText.text = currenHp.ToString("N0");

        if(backHpHit )
        {
            backHpSlider.value = Mathf.Lerp(backHpSlider.value, hpSlider.value, Time.deltaTime * 10f);
            if(hpSlider.value >= backHpSlider.value - 0.01f)
            {
                backHpHit = false;
                backHpSlider.value = hpSlider.value;
            }
        }
    }
    public void Dmg()
    {
        currenHp -= 300f;
        Invoke("BackHpFun", 0.5f);
    }
    private void BackHpFun()
    {
        backHpHit = true;
    }
}
