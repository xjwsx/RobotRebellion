using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpBar : MonoBehaviour
{
    private PlayerController player;
    private HealthSystem healthSystem;
    public Slider hpSlider;
    public Text hpText;
    private void Start()
    {
        player = GameManager.instance.playerController;
        healthSystem = player.GetComponent<HealthSystem>();
        UpdateHpTextUI();
        healthSystem.OnDamage += UpdateHealthUI;
        healthSystem.OnDamage += UpdateHpTextUI;
    }
    void Update()
    {
        hpSlider.value = Mathf.Lerp(hpSlider.value, healthSystem.CurrentHealth / healthSystem.MaxHealth, Time.deltaTime * 5f);
        hpSlider.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 1.1f, player.transform.position.z + 0.5f);
        hpText.transform.position = hpSlider.transform.position;
    }
    private void UpdateHealthUI()
    {
        //hpSlider.value = Mathf.Lerp(hpSlider.value, healthSystem.CurrentHealth / healthSystem.MaxHealth, Time.deltaTime * 5f);
    }
    private void UpdateHpTextUI()
    {
        hpText.text = healthSystem.CurrentHealth.ToString("N0");
    }
    
}
