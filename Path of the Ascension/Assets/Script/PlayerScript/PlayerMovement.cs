using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance {get; private set;}

    [Header("Movement")]
    [SerializeField][Range(1,20)] private float walkSpeed;
    [SerializeField]private float sprintMultiplier = 1.5f;

    [Header("Jump")]
    [SerializeField] private float jumpForce;
    private float gravity = -9.81f;
    //[SerializeField]private bool isJumping = false;

    [Header("Climb")]
    [SerializeField]private bool isClimbing = false;
    [SerializeField] private float climbSpeed;
    private float verticalInput;
    public bool inClimbingState = false;
    public bool canRotate = true;

    [Header("Component")]
    private CharacterController characterController;
    private InputHandler inputHandler;
    private float horizontalInput;
    private Vector3 currentMovement;
    private Animator animator;

    void Awake()
    {
        Instance = this;
    }
    private void Start() 
    {
        animator = GetComponent<Animator>();
        inputHandler = InputHandler.Instance;
        characterController = GetComponent<CharacterController>();
        inputHandler.OnJumpAction += HandleJump;
        inputHandler.OnInteractAction += HandleInteract;
    }

    private void OnDisable() 
    {
        inputHandler.OnJumpAction -= HandleJump;
        inputHandler.OnInteractAction -= HandleInteract;
    }

    private void HandleJump() 
    {
        if (characterController.isGrounded)
        {
            animator.SetBool("isGrounded", true);
            //isJumping = false;
            currentMovement.y = jumpForce;
            animator.SetTrigger("JumpTrigger");
        }
        else
        {
            animator.SetBool("isGrounded",false);
        }
        if(inClimbingState)
        {
            isClimbing = false;
            currentMovement.y = jumpForce;
        }
    }

    private void Update() 
    {
        HandleMovement();
    }

    void HandleMovement() 
    {
        bool isSprinting = inputHandler.SprintValue > 0;
        float speed = walkSpeed * (inputHandler.SprintValue > 0 ? sprintMultiplier : 1f);
        if(isSprinting && characterController.isGrounded)
        {
            animator.SetBool("isSprinting", true);
        }
        else
        {
            animator.SetBool("isSprinting",false);
        }
        horizontalInput = inputHandler.MoveInput.x;
        Vector3 moveDirection = new Vector3(horizontalInput, 0f, 0f);
        moveDirection.Normalize();

        currentMovement.x = moveDirection.x * speed;
        currentMovement.z = 0f;

        if (moveDirection != Vector3.zero && characterController.isGrounded)
        {
            animator.SetBool("isRunning",true);
        }
        else
        {
            animator.SetBool("isRunning",false);
        }

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
        Vector3 position = transform.position;
        characterController.Move(currentMovement * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
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

    void HandleInteract()
    {
        if (inClimbingState)
        {
            isClimbing = !isClimbing;
            canRotate = !canRotate;
        }
    }
}