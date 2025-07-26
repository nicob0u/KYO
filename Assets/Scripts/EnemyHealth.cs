using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyHealth : Health
{
    public Health health;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Enemy enemy;

    public override void Start()
    {
        maxHP = 3;
        base.Start();

        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

        enemy = GetComponent<Enemy>();

    }

    void OnEnable()
    {
        health.onDamageTaken += HandleEnemyDamage;
    }
    void OnDisable()
    {
        health.onDamageTaken -= HandleEnemyDamage;
    }

    public override void Die()
    {
        base.Die();
        enemy.moveTween?.Kill();
        Destroy(gameObject);


    }

    void HandleEnemyDamage(int damage)
    {
            StartCoroutine(FlashRed());

    }

    private IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originalColor;


    }


}
