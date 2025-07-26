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
    public float moveDuration = 3f;
    public float waitDuration = 0.2f;

    [HideInInspector]
    public Tween moveTween;
    private Tween flipTween;
    private Vector3 ogScale;

    public LayerMask groundLayer;

    //GroundCheck
    public Transform edgeCheckPoint; 
    public Vector2 edgeCheckSize = new Vector2(0.2f, 0.2f);

    //WallCheck
    public Transform wallCheckPoint;
    public Vector2 wallCheckSize = new Vector2(0.2f, 0.2f);

    private Transform currentTarget;


    void Start()
    {

        ogScale = transform.localScale;
        currentTarget = pointB;
        MoveToPoint(currentTarget);
    }

    void Update()
    {
        if (gameObject.CompareTag("Slime"))
        {
            if (!IsGroundAhead())
            {
                SwitchTarget();
            }
            if (IsWallAhead())
            {
                SwitchTarget();
            }
        }
       
    }

    bool IsGroundAhead()
    {
      return Physics2D.OverlapBox(edgeCheckPoint.position, edgeCheckSize, 0f, groundLayer);             
    }
    bool IsWallAhead()
    {
        return Physics2D.OverlapBox(wallCheckPoint.position, wallCheckSize, 0f, groundLayer);
   
    }

    void SwitchTarget()
    {
        if (moveTween != null && moveTween.IsActive())
            moveTween.Kill();

        currentTarget = (currentTarget == pointA) ? pointB : pointA;
        MoveToPoint(currentTarget);
    }

    void MoveToPoint(Transform target)
    {
        if (target.position.x > transform.position.x)
            FlipSprite(Mathf.Abs(ogScale.x));
        else
            FlipSprite(-Mathf.Abs(ogScale.x));

        if (moveTween != null && moveTween.IsActive())
            moveTween.Kill();
        moveTween = transform.DOMoveX(target.position.x, moveDuration).SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                DOVirtual.DelayedCall(waitDuration, () =>
                {
                    SwitchTarget();
                });
            });
    }

     void FlipSprite(float targetXScale)
    {
        flipTween?.Kill();
        flipTween = transform.DOScaleX(targetXScale, 0.2f).SetEase(Ease.OutQuad);
    }

     void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        if (edgeCheckPoint != null)
            Gizmos.DrawWireCube(edgeCheckPoint.position, edgeCheckSize);
        Gizmos.color = Color.cyan;
        if (wallCheckPoint != null)
            Gizmos.DrawWireCube(wallCheckPoint.position, wallCheckSize);
    }

   

}

