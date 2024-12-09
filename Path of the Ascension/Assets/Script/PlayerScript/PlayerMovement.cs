using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
[Header("Movement")]
    [SerializeField][Range(1,20)] private float walkSpeed;

    [Header("Jump")]
    [SerializeField] private float jumpForce;
    private float gravity = -9.81f;
    //[SerializeField]private bool isJumping = false;

    [Header("Climb")]
    [SerializeField]private bool isClimbing = false;
    [SerializeField] private float climbSpeed;
    private float verticalInput;
    public bool inClimbingState = false;

    private CharacterController characterController;
    private InputHandler inputHandler;
    private float horizontalInput;
    private Vector3 currentMovement;
    [SerializeField]private bool canRotate = true;

    private void Start() 
    {
        inputHandler = InputHandler.Instance;
        characterController = GetComponent<CharacterController>();
        inputHandler.OnJumpAction += HandleJump;
    }

    private void OnDisable() 
    {
        inputHandler.OnJumpAction -= HandleJump;
    }

    private void HandleJump() 
    {
        if (characterController.isGrounded)
        {
            //isJumping = false;
            currentMovement.y = jumpForce;
        }
        if (inClimbingState)
        {
            isClimbing = !isClimbing;
            canRotate = !canRotate;
        }
    }

    private void Update() 
    {
        HandleMovement();
    }

    void HandleMovement() 
    {
        horizontalInput = inputHandler.MoveInput.x;
        Vector3 moveDirection = new Vector3(horizontalInput, 0f, 0f);
        currentMovement.x = moveDirection.x * walkSpeed;

        if (!characterController.isGrounded)
        {
            currentMovement.y += gravity * Time.deltaTime;
        }
        if(moveDirection != Vector3.zero)
        {
            if (canRotate)
            {
                Quaternion rotation = Quaternion.LookRotation(new Vector3(horizontalInput, 0f, 0f));
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 15f * Time.deltaTime);
            }
        }
        HandleClimb();
        characterController.Move(currentMovement * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            inClimbingState = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            isClimbing = false;
            inClimbingState = false;
            canRotate = true;
        }
    }

    void HandleClimb() 
    {
        if (isClimbing)
        {
            verticalInput = inputHandler.ClimbInput.y;
            Vector3 moveDirection = new Vector3(0f, verticalInput, 0f);
            currentMovement.y = moveDirection.y * climbSpeed;

            if (inClimbingState)
            {
                currentMovement.x = moveDirection.x * 0f;
            }
        }
    }
}
