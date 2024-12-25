using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabState : MonoBehaviour
{
    public static GrabState Instance {get; private set;}
    private InputHandler inputHandler;
    private PlayerMovement playerMovement;
    public bool inGrabingState = false;
    public bool isGrabing = false;

    void Start()
    {
        Instance = this;
        inputHandler = InputHandler.Instance;
        playerMovement = PlayerMovement.Instance;
        inputHandler.OnInteractAction += HandleInteract;
    }
    void OnDisable()
    {
        inputHandler.OnInteractAction -= HandleInteract;
    }

    public void HandleInteract()
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
}
