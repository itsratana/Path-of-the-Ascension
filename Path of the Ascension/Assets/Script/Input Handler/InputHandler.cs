using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset playerControl;

    [Header("Action Map Name Reference")]
    [SerializeField] private string actionMapName = "Player";

    [Header("Action Name Reference")]
    [SerializeField] private string move = "Move";
    [SerializeField] private string jump = "Jump";
    [SerializeField] private string climb = "Climb";
    [SerializeField] private string sprint = "Sprint";
    public event Action OnJumpAction;

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction climbAction;
    private InputAction sprintAction;

    public Vector2 MoveInput { get; private set; }
    public Vector2 ClimbInput { get; private set; }
    public float SprintValue {get; private set;}

    public static InputHandler Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        moveAction = playerControl.FindActionMap(actionMapName).FindAction(move);
        jumpAction = playerControl.FindActionMap(actionMapName).FindAction(jump);
        climbAction = playerControl.FindActionMap(actionMapName).FindAction(climb);
        sprintAction = playerControl.FindActionMap(actionMapName).FindAction(sprint);
        RegisterInputAction();
    }

    void RegisterInputAction()
    {
        moveAction.performed += context => MoveInput = context.ReadValue<Vector2>();
        moveAction.canceled += context => MoveInput = Vector2.zero;

        jumpAction.performed += context => OnJumpAction?.Invoke();        

        climbAction.performed += context => ClimbInput = context.ReadValue<Vector2>();
        climbAction.canceled += context => ClimbInput = Vector2.zero;

        sprintAction.performed += context => SprintValue = context.ReadValue<float>();
        sprintAction.canceled += context => SprintValue = 0f;
    }

    private void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
        climbAction.Enable();
        sprintAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
        climbAction.Disable();
        sprintAction.Disable();
    }
}
