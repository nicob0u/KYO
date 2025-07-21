using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    public Camera cam;
    public Transform player;

    Vector2 startPosition;
    float startZ;

    Vector2 travel => (Vector2)cam.transform.position - startPosition;
    public float parallaxFactor;


    public void Start()
    {
        startPosition = transform.position;
        startZ = transform.position.z;
    }

    public void LateUpdate()
    {
        Vector2 newPos = startPosition + travel * parallaxFactor;
        transform.position = new Vector3(newPos.x, 0, startZ);

    }
}
