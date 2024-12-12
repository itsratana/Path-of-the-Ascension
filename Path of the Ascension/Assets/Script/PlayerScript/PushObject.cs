using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor.Callbacks;
using Unity.VisualScripting;

public class PushObject : MonoBehaviour
{
    private InputHandler inputHandler;
    private PlayerMovement playerMovement;
    public float pushForce = 3f;
    public bool inGrabingState = false;
    [SerializeField]private bool isGrabing = false;

    void Start()
    {
        inputHandler = InputHandler.Instance;
        playerMovement = PlayerMovement.Instance;
        inputHandler.OnInteractAction += HandleInteract;
    }
    void OnDisable()
    {
        inputHandler.OnInteractAction -= HandleInteract;
    }

    void HandleInteract()
    {
        if(inGrabingState)
        {
            isGrabing = !isGrabing;
            playerMovement.canRotate = !playerMovement.canRotate;
        }
    
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 7)
        {
            inGrabingState = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == 7)
        {
            isGrabing = false;
            inGrabingState = false; 
            playerMovement.canRotate = true;
        }
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(isGrabing)
        {
            Rigidbody rb = hit.collider.attachedRigidbody;

            if (rb == null || rb.isKinematic)
                return;

            if (hit.moveDirection.y < -0.3f)
                return;

            Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, 0);

            rb.velocity = pushDir * pushForce;
        }
    }
}
