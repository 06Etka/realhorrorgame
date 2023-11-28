using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Graphics Stuff")]
    [SerializeField] GameObject GFX;

    float speed;
    bool isMoving;

    [Header("Stamina Settings")]
    [SerializeField] float staminaIncreaseAmt;
    float stamina;
    float maxStamina;

    [Header("Walk Settings")]
    [SerializeField] private float walkSpeed;
    bool isWalking;

    [Header("Run Settings")]
    [SerializeField] private float runSpeed;
    [SerializeField] private float runStaminaConsume;
    bool canRun = false;
    bool isRunning;

    [Header("Crouch Settings")]
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float timeToCrouch;
    [SerializeField] Transform cameraTransform;
    [SerializeField] private Transform crouchCheckPosition;
    [SerializeField] private float crouchCheckRadius;
    [SerializeField] Vector3 crouchCameraPosition;
    [SerializeField] float crouchHeight;
    [SerializeField] Vector3 crouchCenter;
    Vector3 uncrouchCameraPosition;
    float uncrouchHeight;
    Vector3 uncrouchCenter;
    bool isCrouching;
    bool canUncrouch = false;
    bool autoUncrouch = false;

    [Header("Jump Settings")]
    [SerializeField] private float jumpHeight = 1f;
    [SerializeField] private float minJumpStaminaConsume;
    [SerializeField] private float maxJumpStaminaConsume;
    bool canJump = false;
    bool hasJumped = false;
    bool hasLanded = false;

    [Header("Keybinds")]
    [SerializeField] private KeyCode runKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;

    [Header("Gravity")]
    [SerializeField] float gravity = -9.81f;
    [SerializeField] Transform groundCheckPos;
    [SerializeField] float groundCheckDistance;

    [Header("References")]
    CameraEffect cameraEffect;

    Vector2 input;
    Vector3 direction;
    Vector3 velocity;

    private CharacterController characterController;

    public MovementState movementState;
    public enum MovementState
    {
        Idle,
        Walking,
        Running,
        Air,
        Crouching
    }

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        Stamina();
        maxStamina = stamina;
        uncrouchCameraPosition = cameraTransform.localPosition;
        uncrouchHeight = characterController.height;
        uncrouchCenter = characterController.center;
        CameraEffect camEffect = GetComponentInChildren<CameraEffect>();
        if(camEffect != null)
        {
            cameraEffect = camEffect;
        }
    }

    private void Update()
    {
        Gravity();
        IsGrounded();
        PlayerInput();
        SetMovementState();
        Speed();
        Move();
        Crouch();
        Jump();
        Land();
        Stamina();
    }

    void Gravity()
    {
        if(IsGrounded() && velocity.y < 0)
        {
            velocity.y = 0f;
        }

        velocity.y += gravity * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);
    }

    bool IsGrounded()
    {
        bool isGrounded;
        isGrounded = Physics.Raycast(groundCheckPos.position, -groundCheckPos.up, groundCheckDistance);
        return isGrounded;
    }

    void PlayerInput()
    {
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        isMoving = input.x > 0.1f || input.y > 0.1f || input.x < -0.1f || input.y < -0.1f;
        if(isMoving && Input.GetKeyDown(runKey) && canRun)
        {
            isRunning = true;
        } else if(Input.GetKeyUp(runKey))
        {
            isRunning = false;
        }

        if (isMoving && !isRunning)
        {
            isWalking = true;
        }
        else isWalking = false;
        Ray ray = new Ray(crouchCheckPosition.position, crouchCheckPosition.up);
        canUncrouch = !Physics.SphereCast(ray, crouchCheckRadius);
        if(!canUncrouch)
        {
            isCrouching = true;
        }
        if (Input.GetKeyDown(crouchKey))
        {
            isCrouching = true;
        }else if(Input.GetKeyUp(crouchKey))
        {
            isCrouching = false;
        }
        if (!canUncrouch && isCrouching)
        {
            autoUncrouch = true;
        }
        if (autoUncrouch && canUncrouch)
        {
            isCrouching = false;
            autoUncrouch = false;
        }
    }

    MovementState SetMovementState()
    {

        if (!IsGrounded()) movementState = MovementState.Air; 
        else if (isCrouching)
        {
            movementState = MovementState.Crouching;
        }
        else if (isMoving)
        {
            if (isRunning)
            {
                movementState = MovementState.Running;
            }
            else movementState = MovementState.Walking;
        }
        else movementState = MovementState.Idle;

        return movementState;
    }

    float Speed()
    {
        switch (movementState)
        {
            case MovementState.Idle:
                break;
            case MovementState.Walking:
                speed = walkSpeed;
                break;
            case MovementState.Running:
                speed = runSpeed;
                break;
            case MovementState.Air:
                break;
            case MovementState.Crouching:
                speed = crouchSpeed;
                break;
            default:
                break;
        }
        return speed;
    }

    Vector3 GetDirection()
    {
        return direction = transform.right * input.x + transform.forward * input.y;
    }

    void Move()
    {
        characterController.Move(GetDirection() * speed * Time.deltaTime);
    }

    void Crouch()
    {
        if(movementState == MovementState.Crouching)
        {
            SetCrouch(crouchHeight, crouchCenter, crouchCameraPosition);
            cameraEffect.SetDefaultY(crouchCameraPosition.y, timeToCrouch);
            GFX.transform.localScale = Vector3.Lerp(GFX.transform.localScale, new Vector3(1f, 0.5f, 1f), timeToCrouch * Time.deltaTime);
        } else
        {
            SetCrouch(uncrouchHeight, uncrouchCenter, uncrouchCameraPosition);
            cameraEffect.SetDefaultY(uncrouchCameraPosition.y, timeToCrouch);
            GFX.transform.localScale = Vector3.Lerp(GFX.transform.localScale, new Vector3(1f, 1f, 1f), timeToCrouch * Time.deltaTime);
        }
    }

    void SetCrouch(float _crouchHeight, Vector3 _crouchCenter, Vector3 _cameraTransform)
    {
        characterController.height = Mathf.Lerp(characterController.height, _crouchHeight, timeToCrouch * Time.deltaTime);
        characterController.center = Vector3.Lerp(characterController.center, _crouchCenter, timeToCrouch * Time.deltaTime);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition,
            new Vector3(_cameraTransform.x, _cameraTransform.y, _cameraTransform.z),
            timeToCrouch * Time.deltaTime);
    }

    void Jump()
    {
        if(IsGrounded() && Input.GetKeyDown(jumpKey) && canJump)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            float randJumpStaminaCost = UnityEngine.Random.Range(minJumpStaminaConsume, maxJumpStaminaConsume);
            PlayerStats.Instance.ConsumeStamina(randJumpStaminaCost, false);
            cameraEffect.shakeSpeed = 9f;
            cameraEffect.Shake(-10f, 10f, 8f);
            print("jump camera effect!");
            Invoke(nameof(CheckJump), 0.2f);
        }
    }

    bool Land()
    {
        if (hasJumped && IsGrounded())
        {
            cameraEffect.shakeSpeed = 13f;
            cameraEffect.Shake(-15f, -15f, 12f);
            print("land camera effect!");
            hasJumped = false;
            return true;
        }
        else return false;
    }

    void CheckJump()
    {
        if(!IsGrounded())
        {
            hasJumped = true;
        }
    }

    void Stamina()
    {
        stamina = PlayerStats.Instance.stamina;
        if(stamina < runStaminaConsume)
        {
            canRun = false;
            isRunning = false;
        } else { canRun = true; }
        if(stamina < minJumpStaminaConsume)
        {
            canJump = false;
        } else { canJump = true; }
        if(movementState == MovementState.Running)
        {
            PlayerStats.Instance.ConsumeStamina(runStaminaConsume, true);
        } else if(movementState != MovementState.Running && stamina < maxStamina) 
        { 
            PlayerStats.Instance.RegainStamina(staminaIncreaseAmt, true); 
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(crouchCheckPosition.position, crouchCheckRadius);
    }
}