using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class DamageFeedback : MonoBehaviour
{
    public Health health;
    public SpriteRenderer spriteRenderer;
    private PlayerController player;
    private Rigidbody2D rb;

    public float invincibilityDuration = 2f;
    public float flickerInterval = 0.1f;

    bool isInvincible = false;

    public float pushForce = 5f;


  

    void OnEnable()
    {
        health.onDamageTaken += HandleDamageTaken;
    }
    void OnDisable()
    {
        health.onDamageTaken -= HandleDamageTaken;
    }

    void Start()
    {
        player = GetComponent<PlayerController>();
        rb = player.GetComponent<Rigidbody2D>();
      

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
            player.enableGroundCheck = false;
            player.enableRoofCheck = false;
            yield return new WaitForSeconds(flickerInterval);
            elapsed += flickerInterval;

        }

        spriteRenderer.enabled = true;
        isInvincible = false;
        player.enableGroundCheck = true;
        player.enableRoofCheck = true;



        gameObject.layer = originalLayer;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Slime") || collision.CompareTag("Ghost"))
        {
            Vector2 direction = (transform.position - collision.transform.position).normalized;
          

            rb.velocity = Vector2.zero; 
            rb.AddForce(direction * pushForce, ForceMode2D.Impulse);

            player.knockbackTimer = player.knockbackDuration; 
        }
    }

}