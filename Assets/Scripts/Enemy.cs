using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;

using System;
using System.Security.Cryptography;
using System.ComponentModel.Design;
using UnityEngine;
using DG.Tweening;
using System.Diagnostics;

public class Enemy : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float moveSpeed = 4f;
    public float waitDuration = 0.2f;

    private Vector3 ogScale;

    public LayerMask groundLayer;

    //GroundCheck
    public Transform edgeCheckPoint;
    public Vector2 edgeCheckSize = new Vector2(0.2f, 0.2f);

    //WallCheck
    public Transform wallCheckPoint;
    public Vector2 wallCheckSize = new Vector2(0.2f, 0.2f);

    private Transform currentTarget;

    private bool isWaiting = false;

    private Rigidbody2D rb;
    private EnemyHealth enemyHealth;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
            rb = GetComponentInChildren<Rigidbody2D>();

        enemyHealth = GetComponent<EnemyHealth>();
        if (enemyHealth == null)
            enemyHealth = GetComponentInChildren<EnemyHealth>();

        ogScale = transform.localScale;
        currentTarget = pointB;
    }

    void FixedUpdate()
    {

        if (!isWaiting)
        {
            Patrol();
        }

    }

    bool IsGroundAhead()
    {
        if (wallCheckPoint == null) return false;
        return Physics2D.OverlapBox(edgeCheckPoint.position, edgeCheckSize, 0f, groundLayer);
    }
    bool IsWallAhead()
    {
        if (edgeCheckPoint == null) return false;
        return Physics2D.OverlapBox(wallCheckPoint.position, wallCheckSize, 0f, groundLayer);

    }

    void SwitchTarget()
    {


        currentTarget = (currentTarget == pointA) ? pointB : pointA;
    }

    void Patrol()
    {

        if (enemyHealth.isKnockedBack)
        {
            return;
        }
        else
        {

            if (currentTarget.position.x > transform.position.x)
            {
                transform.localScale = new Vector3(Mathf.Abs(ogScale.x), ogScale.y, ogScale.z);
            }
            else
            {
                transform.localScale = new Vector3(-Mathf.Abs(ogScale.x), ogScale.y, ogScale.z);
            }

            Vector3 targetPosition = Vector3.MoveTowards(transform.position, currentTarget.position, moveSpeed * Time.deltaTime);
            rb.MovePosition(targetPosition);

            if (!isWaiting && Vector3.Distance(transform.position, currentTarget.position) < 1)
            {
                StartCoroutine(WaitThenSwitch());
            }

            if (gameObject.CompareTag("Slime"))
            {
                if (!isWaiting && (!IsGroundAhead() || IsWallAhead()))
                {
                    StartCoroutine(WaitThenSwitch());
                }

            }

            if (gameObject.CompareTag("Ghost"))
            {
                if (!isWaiting && (IsWallAhead()))
                {
                    StartCoroutine(WaitThenSwitch());
                }

            }
        }


    }

    IEnumerator WaitThenSwitch()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitDuration);
        SwitchTarget();
        isWaiting = false;
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        if (edgeCheckPoint != null)
            Gizmos.DrawWireCube(edgeCheckPoint.position, edgeCheckSize);
        Gizmos.color = Color.cyan;
        if (wallCheckPoint != null)
            Gizmos.DrawWireCube(wallCheckPoint.position, wallCheckSize);
    }



}

