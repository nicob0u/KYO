using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadDamageCollision : MonoBehaviour
{
    public int attackPower = 1;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && collision.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {

            Vector2 contactNormal = collision.GetContact(0).normal;
            if (contactNormal.y < -0.5)
            {
                var enemyHealth = GetComponentInParent<Health>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(attackPower);
                    UnityEngine.Debug.Log("Enemy hit");

                }

                Rigidbody2D playerRb = collision.collider.GetComponent<Rigidbody2D>();
                if (playerRb != null)

                    playerRb.velocity = new Vector2(playerRb.velocity.x, 8f);


            }


        }

    }
}
