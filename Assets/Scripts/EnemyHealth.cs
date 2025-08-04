using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class EnemyHealth : Health
{
    public Health health;
    public float pushForce = 5f;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Enemy enemy;
    private GameObject player;
    private Rigidbody2D rb;
    private Collider2D coll;
    private Tween fadeTween;

    public bool isKnockedBack = false;


    public override void Start()
    {
        maxHP = 3;
        base.Start();

        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
            rb = GetComponentInParent<Rigidbody2D>();

        enemy = GetComponent<Enemy>();
        player = GameObject.FindWithTag("Player");

        coll = GetComponent<Collider2D>();


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
        if (gameObject != null)
        {
            StartCoroutine(DeathSequence());

        }


    }

    void HandleEnemyDamage(int damage)
    {

        StartCoroutine(DamageFeedback());
    }

    IEnumerator DamageFeedback()
    {
        Vector2 direction = (enemy.transform.position - player.transform.position).normalized;
        Vector2 force = direction * pushForce;

        isKnockedBack = true;
        spriteRenderer.color = Color.red;
        rb.AddForce(force, ForceMode2D.Impulse);


        yield return new WaitForSeconds(0.9f);

        spriteRenderer.color = originalColor;
        isKnockedBack = false;

    }

    IEnumerator DeathSequence()
    {
        coll.enabled = false;
        spriteRenderer.color = Color.yellow;
        rb.constraints = RigidbodyConstraints2D.None;

        yield return transform.DOScale(1.3f, 0.1f).SetLoops(2, LoopType.Yoyo).WaitForCompletion();

        Sequence drop = DOTween.Sequence();
        drop.Append(transform.DOMoveY(transform.position.y - 1f, 0.5f));
        drop.Join(spriteRenderer.DOFade(0f, 0.5f));
        yield return drop.WaitForCompletion();

        Destroy(gameObject);
    }


}
