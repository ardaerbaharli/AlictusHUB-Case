using System;
using Player.Data;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    public float maxHealth;
    public float currentHealth;

    public Action OnDied;

    public float MaxHealth
    {
        get => maxHealth;
        set
        {
            maxHealth = value;
            UpdateHealthBar();
        }
    }

    private float CurrentHealth
    {
        get => currentHealth;
        set
        {
            currentHealth = value;
            UpdateHealthBar();
        }
    }

    private void UpdateHealthBar()
    {
        healthBar.value = CurrentHealth / MaxHealth;
    }

    public void Heal(int healAmount)
    {
        CurrentHealth += healAmount;
        if (CurrentHealth > MaxHealth) CurrentHealth = MaxHealth;
    }

    public void SetHealthData(PlayerHealthData healthData)
    {
        MaxHealth = healthData.maxHealth;
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            OnDied?.Invoke();
        }
    }
}