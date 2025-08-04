using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;

using System;
using System.Security.Cryptography;
using System.ComponentModel.Design;
using UnityEngine;
using DG.Tweening;
using System.Diagnostics;
using UnityEngine.Tilemaps;

public class Enemy : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float moveSpeed = 4f;
    public float waitDuration = 0.2f;

    private Vector3 ogScale;

    public LayerMask groundLayer;
    public LayerMask playerLayer;

    //Ground check
    public Transform edgeCheckPoint;
    public Vector2 edgeCheckSize = new Vector2(0.2f, 0.2f);

    //Wall check
    public Transform wallCheckPoint;
    public Vector2 wallCheckSize = new Vector2(0.2f, 0.2f);

    private Transform currentTarget;

    private bool isWaiting = false;
    private bool isTargetTooFar = false;

    private Rigidbody2D rb;
    private EnemyHealth enemyHealth;

    public Transform spikeCheck;
    public Vector2 spikeCheckSize = new Vector2(0.5f, 0.5f);

    public List<TileBase> spikeTiles;
    private Tilemap tilemap;
    private bool canTakeDamage = true;



    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
            rb = GetComponentInChildren<Rigidbody2D>();

        enemyHealth = GetComponent<EnemyHealth>();
        if (enemyHealth == null)
            enemyHealth = GetComponentInChildren<EnemyHealth>();

        tilemap = GameObject.Find("Grid/Obstacles").GetComponent<Tilemap>();
        if (tilemap == null)
        {
            UnityEngine.Debug.Log("Couldn't get tilemap.");

        }


        ogScale = transform.localScale;
        currentTarget = pointB;


    }

    void FixedUpdate()
    {

        if (!isWaiting && !enemyHealth.isKnockedBack)
        {
            Patrol();
        }

        if (gameObject.CompareTag("Slime"))
        {
            DealSpikeDamage();
        }

    }

    void DealSpikeDamage()
    {
        Vector3Int cellPos = tilemap.WorldToCell(spikeCheck.position);
        TileBase tile = tilemap.GetTile(cellPos);

        if (spikeTiles.Contains(tile) && canTakeDamage)
        {
            enemyHealth.TakeDamage(1);
            canTakeDamage = false;
            StartCoroutine(DamageCoolDown());

        }

    }

    IEnumerator DamageCoolDown()
    {
        yield return new WaitForSeconds(0.5f);
        canTakeDamage = true;
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
        if (isTargetTooFar)
        {
            pointA.position = new Vector3(transform.position.x - 3f, transform.position.y, transform.position.z);
            pointB.position = new Vector3(transform.position.x + 3f, transform.position.y, transform.position.z);
        }

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

            if (Vector3.Distance(transform.position, currentTarget.position) >= 10f)
            {
                isTargetTooFar = true;
                UnityEngine.Debug.Log("target too far");
                StartCoroutine(WaitThenSwitch());


            }

        }


    }

    IEnumerator WaitThenSwitch()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitDuration);
        SwitchTarget();
        isWaiting = false;

        if (isTargetTooFar)
            isTargetTooFar = false;
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        if (edgeCheckPoint != null)
            Gizmos.DrawWireCube(edgeCheckPoint.position, edgeCheckSize);
        Gizmos.color = Color.cyan;
        if (wallCheckPoint != null)
            Gizmos.DrawWireCube(wallCheckPoint.position, wallCheckSize);
        Gizmos.color = Color.yellow;
        if (spikeCheck != null)
            Gizmos.DrawWireCube(spikeCheck.position, spikeCheckSize);
        Gizmos.DrawWireCube(wallCheckPoint.position, wallCheckSize);
       
    }

    


}

