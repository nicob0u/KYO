using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Health : MonoBehaviour
{
    protected int maxHP;
    protected int currentHP;
    private bool isDead = false;

    public delegate void OnDamageTaken(int damage);
    public event OnDamageTaken onDamageTaken;

    public virtual void Start()
    {
        currentHP = maxHP;
    }


    public virtual void TakeDamage(int damage)
    {
        if (isDead) return;
        currentHP -= damage;
        onDamageTaken?.Invoke(damage);
        UnityEngine.Debug.Log("Health: " + currentHP);

        if (currentHP <= 0)
        {
            Die();
        }

    }

    public virtual void Die()
    {
        isDead = true;

    }
}
