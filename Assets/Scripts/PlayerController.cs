using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.Tilemaps;
using System.Linq;
using System.Diagnostics;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator anim;
    private Vector3 ogScale;
    private Tween flipTween;

    //Movement
    [HideInInspector]
    public float horizontalMovement;

    public float moveSpeed = 5f;

    //Jumping
    public float jumpForce = 8f;
    public int maxJumps = 2;
    private int jumpsRemaining;
    private Tween scaleYTween;


    //GroundCheck
    public Transform groundCheckPoint;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.5f);
    public LayerMask groundLayer;

    //Gravity
    public float baseGravity = 2;
    public float maxFallSpeed = 18f;
    public float fullSpeedMultiplier = 2f;

    //Attack
    private PlayerInput input;
    private InputAction attackAction;
    public Transform attackPoint;
    public float attackRadius;
    public LayerMask enemyLayer;
    private bool isGrounded;

    //Roof spike detection
    public Transform spikeCheck;
    public Vector2 spikeCheckSize = new Vector2(0.5f, 0.5f);
    [SerializeField]
    public bool isTouchingRoofSpike = false;

    [HideInInspector]
    public bool isAttacking = false;
    [SerializeField]
    public bool enableGroundCheck = true;
    public bool enableRoofCheck = true;


    public List<TileBase> spikeTiles;
    private Tilemap tilemap;

    void Start()
    {
        input = GetComponent<PlayerInput>();
        attackAction = input.actions.FindAction("Attack");
        attackAction.performed += Attack;


        jumpsRemaining = maxJumps;
        ogScale = transform.localScale;

        tilemap = GameObject.Find("Obstacles").GetComponent<Tilemap>();
    }

    void Update()
    {

        rb.velocity = new Vector2(horizontalMovement * moveSpeed, rb.velocity.y);


        GroundCheck();
        RoofSpikeCheck();
        Gravity();
    }

    void FixedUpdate()
    {
        anim.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("yVelocity", rb.velocity.y);
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;

        if (horizontalMovement > 0)
        {
            FlipSprite(Mathf.Abs(ogScale.x));

            anim.SetBool("isWalking", true);

        }
        else if (horizontalMovement < 0)
        {
            FlipSprite(-Mathf.Abs(ogScale.x));

            anim.SetBool("isWalking", true);


        }
        else
        {
            anim.SetBool("isWalking", false);

        }

    }

    private void FlipSprite(float targetXScale)
    {
        flipTween?.Kill();

        flipTween = transform.DOScaleX(targetXScale, 0.2f)
                             .SetEase(Ease.OutQuad);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (jumpsRemaining > 0)
        {
            if (context.performed)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpsRemaining--;

                anim.SetBool("isJumping", true);

                scaleYTween?.Kill();
                scaleYTween = transform.DOScaleY(ogScale.y * 1.2f, 0.1f)
                       .SetEase(Ease.OutQuad)
                       .OnComplete(() =>
                           transform.DOScaleY(ogScale.y, 0.1f)
                                    .SetEase(Ease.InQuad)
                       );
            }
            else if (context.canceled)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
                jumpsRemaining--;
            }
        }
    }



    private void GroundCheck()
    {
        bool wasGrounded = isGrounded;

        isGrounded = Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, groundLayer);

        if (isGrounded)
        {
            Vector3Int worldPos = tilemap.WorldToCell(groundCheckPoint.position);
            TileBase tile = tilemap.GetTile(worldPos);

            if (spikeTiles.Contains(tile) && enableGroundCheck)
            {
                var playerHealth = GetComponent<Health>();
                playerHealth.TakeDamage(1);
            }

            jumpsRemaining = maxJumps;
        }

        if (!wasGrounded && isGrounded && enableGroundCheck)
        {
            anim.SetBool("isJumping", false);
            anim.SetBool("isFalling", false);

            scaleYTween?.Kill();
            scaleYTween = transform.DOScaleY(ogScale.y * 0.8f, 0.05f)
                      .SetEase(Ease.OutQuad)
                      .OnComplete(() =>
                          transform.DOScaleY(ogScale.y, 0.05f)
                                   .SetEase(Ease.InQuad)
                      );
        }
    }

    void RoofSpikeCheck()
    {
        isTouchingRoofSpike = Physics2D.OverlapBox(spikeCheck.position, spikeCheckSize, 0, groundLayer);
        Vector3Int worldPos = tilemap.WorldToCell(spikeCheck.position + Vector3.up * 0.5f);
        TileBase tile = tilemap.GetTile(worldPos);
    

        if (tile != null && spikeTiles.Any(t => t != null && t.name == tile.name) && isTouchingRoofSpike && enableRoofCheck)
        {
            var playerHealth = GetComponent<Health>();
            playerHealth.TakeDamage(1);
        }


    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed && !isAttacking)
        {
            isAttacking = true;
            anim.SetBool("isAttacking", true);

        }
    }

    public void AttackAtPoint()
    {


        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, enemyLayer);

        foreach (Collider2D enemyGameObject in enemies)
        {
            var enemyHealth = enemyGameObject.GetComponent<Health>();
            enemyHealth.TakeDamage(1);
            UnityEngine.Debug.Log("Enemy hit");

        }

    }

    public void EndAttack()
    {
        isAttacking = false;
        anim.SetBool("isAttacking", false);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(groundCheckPoint.position, groundCheckSize);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(spikeCheck.position, spikeCheckSize);


    }

    private void Gravity()
    {
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = baseGravity * fullSpeedMultiplier;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed));
            anim.SetBool("isJumping", false);
            anim.SetBool("isFalling", true);
        }
        else
        {
            rb.gravityScale = baseGravity;
            if (rb.velocity.y > 0.1f)
            {
                anim.SetBool("isJumping", true);
                anim.SetBool("isFalling", false);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Slime") || other.CompareTag("Ghost"))
        {

            var playerHealth = gameObject.GetComponent<Health>();

            if (playerHealth == null)
            {
                UnityEngine.Debug.LogWarning("PlayerHealth missing.");
                return;
            }
            playerHealth.TakeDamage(1);


        }
    }


}