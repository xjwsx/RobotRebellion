using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class EnemyHpBar : MonoBehaviour
{
    [SerializeField] private EnemyController enemy;
    private HealthSystem healthSystem;
    public Slider hpSlider;
    public Text hpText;

    private void Start()
    {
        healthSystem = enemy.GetComponent<HealthSystem>();
        UpdateHpTextUI();
        healthSystem.OnDamage += UpdateHealthUI;
        healthSystem.OnDamage += UpdateHpTextUI;
        healthSystem.OnDeath += SetActive;
    }
    void Update()
    {
        hpSlider.transform.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y + 1f, enemy.transform.position.z);
        hpText.transform.position = hpSlider.transform.position;
    }
    private void UpdateHealthUI()
    {
        hpSlider.value = Mathf.Lerp(hpSlider.value, healthSystem.CurrentHealth / healthSystem.MaxHealth, Time.deltaTime * 5f);
    }
    private void UpdateHpTextUI()
    {
        hpText.text = healthSystem.CurrentHealth.ToString("N0");
    }
    public void SetActive()
    {
        gameObject.SetActive(false);
    }
}
