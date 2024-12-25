using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 2f; 
    public float height = 2f;
    public bool startMovingUp = true;
    private Vector3 startPosition;
    private float initialOffset;

    void Start()
    {
        startPosition = transform.position;
        initialOffset = startMovingUp ? 0f : Mathf.PI;
    }

    void Update()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * speed + initialOffset) * height;

        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
