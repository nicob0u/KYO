using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFeedback : MonoBehaviour
{
    public Health health;
    public SpriteRenderer spriteRenderer;

    public float invincibilityDuration = 2f;
    public float flickerInterval = 0.1f;

    bool isInvincible = false;

    void OnEnable()
    {
        health.onDamageTaken += HandleDamageTaken;
    }
    void OnDisable()
    {
        health.onDamageTaken -= HandleDamageTaken;
    }

    void HandleDamageTaken(int damage)
    {
        if (!isInvincible)
        {
            StartCoroutine(InvincibilityCoroutine());
        }
    }

    public IEnumerator InvincibilityCoroutine()
    {
        int originalLayer = gameObject.layer;

        gameObject.layer = LayerMask.NameToLayer("PlayerInvincible");

        isInvincible = true;

        float elapsed = 0;
        while (elapsed < invincibilityDuration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(flickerInterval);
            elapsed += flickerInterval;

        }

        spriteRenderer.enabled = true;
        isInvincible = false;

        gameObject.layer = originalLayer;
    }
}