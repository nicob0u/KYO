using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraFollow : MonoBehaviour
{
    private PlayerController playerController;
    private int facingDirection = 1;

    public Transform target;
    public float pixelsPerUnit = 32f;
    public Vector2 baseOffset;
    public Vector2 offset
    {
        get
        {     
            return baseOffset * facingDirection;
        }
    }
    public float smoothTime = 0.2f;

    private Tween moveTween;
    private Camera cam;

    void Awake()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + (Vector3)offset;
        desiredPosition.z = transform.position.z;

        float unitsPerPixel = 1f / pixelsPerUnit;

        float snappedX = Mathf.Round(desiredPosition.x / unitsPerPixel) * unitsPerPixel;
        Vector3 snappedPosition = new Vector3(snappedX, 0, desiredPosition.z);

        if (moveTween != null && moveTween.IsActive())
        {
            moveTween.Kill();
        }
        moveTween = transform.DOMove(snappedPosition, smoothTime);

        float hMove = playerController.horizontalMovement;
        if (hMove > 0)
            facingDirection = 1;
        else if (hMove < 0)
            facingDirection = -1;
  
    }

}
