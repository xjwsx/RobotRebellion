using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event Action OnDamage;
    public event Action OnDeath;
    //public event Action OnHealth;
    public bool IsDead = false;

    public float CurrentHealth { get; private set; }
    public float MaxHealth {get; private set; }

    public void SetUp(float maxHealth)
    {
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
    }
    public bool ChangeHealth(float change)
    {
        if (change == 0) return false;

        CurrentHealth += change;
        //CurrentHealth = CurrentHealth > MaxHealth ? MaxHealth : CurrentHealth;
        //CurrentHealth = CurrentHealth < 0 ? 0 : CurrentHealth;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);

        OnDamage?.Invoke();

        if (CurrentHealth <= 0f)
        {
            IsDead = true;
            OnDeath?.Invoke();
        }
        return true;
    }
}
