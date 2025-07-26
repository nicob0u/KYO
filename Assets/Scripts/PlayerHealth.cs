using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    public override void Start()
    {
        maxHP = 5;
        base.Start();
    }


    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);   
    }

    public override void Die()
    {
        base.Die();
        UnityEngine.Debug.Log("Game over");


    }
}
