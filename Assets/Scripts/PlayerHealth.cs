using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
        UnityEngine.Debug.Log("Health: " + currentHP);

    }

    public override void Die()
    {


        base.Die();
        DOTween.Kill(gameObject);
        DOTween.Kill(transform);
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) DOTween.Kill(sr);


        UnityEngine.Debug.Log("Game over");
        GameManager.Instance.GameOver();


    }


}
