using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor.Callbacks;
using Unity.VisualScripting;

public class PushObject : MonoBehaviour
{
    public static PushObject Instance {get; private set;}
    public Transform playerTransform;
    public float pushPullSpeed = 3f;
    private Vector3 offset;
    private bool isInteracting = false;

    void Start()
    {
        //InputHandler.Instance.OnInteractAction += HandlePushPull;
    }
     void Update()
    {
        if (GrabState.Instance.isGrabing && !isInteracting)
        {
            StartInteraction();
        }
        else if (!GrabState.Instance.isGrabing && isInteracting)
        {
            StopInteraction();
        }
        if (isInteracting)
        {
            HandlePushPull();
        }
    }

    public void StartInteraction()
    {
        // Begin interaction and calculate the offset
        isInteracting = true;
        offset = transform.position - playerTransform.position;
    }

    public void StopInteraction()
    {
        // End interaction
        isInteracting = false;
    }

    private void HandlePushPull()
    {
        // Calculate the new position based on the player's position and offset
        Vector3 targetPosition = playerTransform.position + offset;

        // Adjust the position based on player input
        float horizontal = Input.GetAxis("Horizontal"); // Replace with custom input if needed

        // Add movement in the forward/backward direction of the player
        Vector3 direction = new Vector3(horizontal, 0, 0).normalized;
        targetPosition += direction * pushPullSpeed * Time.deltaTime;

        // Update the object's position
        transform.position = targetPosition;
    }
}