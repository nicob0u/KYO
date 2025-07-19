using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using DG.Tweening;
using System;
using System.Security.Cryptography;

public class EnemyMovement : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float moveDuration = 3f;
    public float waitDuration = 0.2f;
    private Tween moveTween;
    private Tween flipTween;
    private Vector3 ogScale;


    void Start()
    {
        ogScale = transform.localScale;
        MoveToPoint(pointB);
    }

    void MoveToPoint(Transform target)
    {

        if (target.position.x > transform.position.x)
            FlipSprite(Math.Abs(ogScale.x));
        else
            FlipSprite(-Math.Abs(ogScale.x));


        if (moveTween != null && moveTween.IsActive())
        {
            moveTween.Kill();
        }

          moveTween =  transform.DOMoveX(target.position.x, moveDuration).SetEase(Ease.Linear)
                .OnComplete(() =>
                {

                    DOVirtual.DelayedCall(waitDuration, () =>
                   {
                       MoveToPoint(target == pointA ? pointB : pointA);
                       
                   });
                });
        

    }
    private void FlipSprite(float targetXScale)
    {
        flipTween?.Kill();

        flipTween = transform.DOScaleX(targetXScale, 0.2f)
                             .SetEase(Ease.OutQuad);
    }
}
