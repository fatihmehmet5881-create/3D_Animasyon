using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private Joystick joystick;
    [SerializeField] private float WalkSpeed = 4f;
    [SerializeField] private float SprintSpeed = 7f;
    [SerializeField] private float JumpHeight = 1.5f;
    [SerializeField] private float Gravity = -9.81f;
    [SerializeField] private Transform GroundCheck;
    [SerializeField] private float GroundDistance = 0.25f;
    [SerializeField] private LayerMask GroundMask;
    [SerializeField] private Animator animator;

    private Vector2 moveInput;
    private Vector3 verticalVelocity;
    private bool isGrounded;

    private void Reset()
    {
        controller = GetComponent<CharacterController>();
    }
    private void Awake()
    {
        if(controller == null) controller = GetComponent<CharacterController>();
        if(animator == null) animator = GetComponentInChildren<Animator>();
    }
    private void Update()
    {
        HandleGroundCheck();
        ReadJoystickInput();
        HandleHorizontalMovement();
        HandleGravity();
        if(Keyboard.current.spaceKey.wasPressedThisFrame) Jump();
        UpdateAnimations();
    }

    private void HandleGroundCheck()
    {
        isGrounded = Physics.CheckSphere(GroundCheck.position, GroundDistance, GroundMask);
        if(isGrounded && verticalVelocity.y < 0) verticalVelocity.y = -2f;
    }
    private void ReadJoystickInput()
    {
        if(joystick != null)
        {
            moveInput.x = joystick.Horizontal;
            moveInput.y = joystick.Vertical;
        }
    }
    private void HandleHorizontalMovement()
    {
        Vector3 moveDirection = transform.right * moveInput.x + transform.forward * moveInput.y;
        bool isSprinting = 0.85f < moveInput.magnitude;
        float currentSpeed = isSprinting ? SprintSpeed : WalkSpeed;
        controller.Move(moveDirection* (currentSpeed* Time.deltaTime));
    }
    private void HandleGravity()
    {
        verticalVelocity.y += Gravity* Time.deltaTime;
        controller.Move(verticalVelocity* Time.deltaTime);
    }
    private void UpdateAnimations()
    {
        if (animator == null) return;

        bool isMoving = moveInput.magnitude > 0.01f;
        bool isSprinting = isMoving && moveInput.magnitude > 0.85f;

        bool isWalking = isMoving && !isSprinting;
        bool isRunning = isSprinting;
        bool isIdle = !isMoving && isGrounded;

        animator.SetBool("isIdle", isIdle);
        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isRunning", isRunning);

        if (isGrounded && verticalVelocity.y <= 0) animator.SetBool("isJumping", false);
    }

    public void Jump()
    {
        if(isGrounded)
        {
            verticalVelocity.y = Mathf.Sqrt(JumpHeight * -2f * Gravity);
            if(animator != null) animator.SetBool("isJumping", true);
        }
    }
}