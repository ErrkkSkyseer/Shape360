using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] int maxHP;
    int currentHP;

    public int HP { get => currentHP; }

    public UnityAction OnTakeDamage;
    public UnityAction OnDead;

    private void Start()
    {
        currentHP = maxHP;
    }
    public bool IsAlive() => currentHP > 0;
    public void TakeDamage(int damage)
    {
        if (!IsAlive())
            return;

        currentHP -= damage;
        if (currentHP <= 0)
        {
            currentHP = 0;
            OnDead?.Invoke();
        }
        else
        {
            OnTakeDamage?.Invoke();
        }
    }

    public void Heal(int heal)
    {
        if (!IsAlive())
            return;

        currentHP += heal;
        
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
    }

    public void Kill()
    {
        if (!IsAlive())
            return;

        currentHP = 0;
        OnDead?.Invoke();
    }
    public void Reset()
    {
        currentHP = maxHP;
    }
}
