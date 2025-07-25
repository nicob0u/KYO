using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

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

    //GroundCheck
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.5f);
    public LayerMask groundLayer;

    //Gravity
    public float baseGravity = 2;
    public float maxFallSpeed = 18f;
    public float fullSpeedMultiplier = 2f;

  
    private PlayerInput playerInput;
    private InputAction attackAction;


    private bool isGrounded;

    private void Awake()
    {
       
    }

    void Start()
    {
        jumpsRemaining = maxJumps;
        ogScale = transform.localScale;
    }

    void Update()
    {
      
        rb.velocity = new Vector2(horizontalMovement * moveSpeed, rb.velocity.y);
        

        GroundCheck(); 

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
        }
        else if (horizontalMovement < 0)
        {
            FlipSprite(-Mathf.Abs(ogScale.x));

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

                transform.DOScaleY(ogScale.y * 1.2f, 0.1f)
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

        isGrounded = Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer);

        if (isGrounded)
        {
            jumpsRemaining = maxJumps;

            if (!wasGrounded)
            {
                anim.SetBool("isJumping", false);
                anim.SetBool("isFalling", false);

                transform.DOScaleY(ogScale.y * 0.8f, 0.05f)
                         .SetEase(Ease.OutQuad)
                         .OnComplete(() =>
                             transform.DOScaleY(ogScale.y, 0.05f)
                                      .SetEase(Ease.InQuad)
                         );
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);

      
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