using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadDamageTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {

        int playerPoweredUpLayer = LayerMask.NameToLayer("PlayerPoweredUp");
        if (other.gameObject.layer != playerPoweredUpLayer)
            return;

        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerPoweredUp"))
        {
            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();
            if (playerRb == null) return;

            if (playerRb.velocity.y <= 0f && other.transform.position.y > transform.position.y)
            {
                var enemyHealth = GetComponentInParent<Health>();
                if (enemyHealth != null)
                    enemyHealth.Die();

                playerRb.velocity = new Vector2(playerRb.velocity.x, 8f);
            }
        }


    }
}
