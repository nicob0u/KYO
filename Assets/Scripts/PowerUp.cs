using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PowerUp : MonoBehaviour
{
    public float powerUpDuration = 10f;
    private PlayerController player;
    private SpriteRenderer spriteRenderer;
    private Vector3 originalScale;
    private Color originalColor;

    public float bounceDuration = 0.3f;
    public float glowDuration = 1.0f;
    public Color glowColor = new Color(1f, 1f, 0.5f, 1f);
    private float originalXScale;
    private float originalSpeed;
    private int originalPower;

    private Tween punchTween;
    private Sequence seq;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();


        spriteRenderer = player.GetComponent<SpriteRenderer>();

        originalScale = player.transform.localScale;
        originalColor = spriteRenderer.color;
        originalXScale = Mathf.Sign(originalScale.x);
        originalSpeed = player.moveSpeed;
        originalPower = player.attackPower;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            StartCoroutine(ActivatePowerUp());
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<Collider2D>().enabled = false;
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;


        }
    }

    public IEnumerator ActivatePowerUp()
    {
        if (player == null || spriteRenderer == null)
            yield break;

        //Power up boost
        player.moveSpeed = 15f;
        player.attackPower = 3;

        //Invincibility settings
        int originalLayer = player.gameObject.layer;
        player.gameObject.layer = LayerMask.NameToLayer("PlayerPoweredUp");

        bool isPunchOver = false;

        //Power up effects
        float elapsed = 0;
        while (elapsed < powerUpDuration)
        {
            if (player == null || spriteRenderer == null)
                yield break;

            float direction = player.facingDirection;

            //Scale punch effect
            punchTween = player.transform.DOPunchScale(new Vector3(0f, 0.3f, 0f), 0.3f, 10, 1f)
              .OnComplete(() =>
                      {

                          Vector3 scale = player.transform.localScale;
                          player.transform.localScale = new Vector3(
                            player.facingDirection * Mathf.Abs(originalScale.x), 
                            originalScale.y,
                            originalScale.z
                        );
                          isPunchOver = true;
                      });
            player.enableGroundCheck = false;
            player.enableRoofCheck = false;

            //Glow effect
            bool isGlowOver = false;
            seq = DOTween.Sequence();

            if (spriteRenderer == null || player == null)
            {
                yield break;
            }
            else
            {
                seq.Append(spriteRenderer.DOColor(glowColor, bounceDuration));
                seq.Append(spriteRenderer.DOColor(originalColor, glowDuration).SetEase(Ease.InOutSine))
                    .OnComplete(() =>
                    {
                        if (spriteRenderer != null)
                            isGlowOver = true;
                    });

            }

            yield return new WaitUntil(() => isPunchOver && isGlowOver);
            elapsed += bounceDuration + glowDuration;
            isPunchOver = false;

        }

        //Back to original settings
        player.moveSpeed = originalSpeed;
        player.attackPower = originalPower;
        player.gameObject.layer = originalLayer;
        player.enableGroundCheck = true;
        player.enableRoofCheck = true;


    }

    private void OnDestroy()
    {
        if (punchTween != null && punchTween.IsActive())
            punchTween.Kill();

        if (seq != null && seq.IsActive())
            seq.Kill();

        if (spriteRenderer != null)
            DOTween.Kill(spriteRenderer);

        if (player != null && player.transform != null)
            DOTween.Kill(player.transform);
    }


}
