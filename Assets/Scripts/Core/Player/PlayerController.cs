using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Cinemachine;
using UnityEngine.TextCore.Text;

public class PlayerController : MonoBehaviour
{
    public CharacterController characterController;
    
    [Header("Functional Options")]
    public bool canMove;

    public bool canSprint;
    public bool canMouseLook;
    public bool headBobEnabled; 
    [Header("Movement Settings")] 
    public float sprintSpeed, walkSpeed,crouchSpeed, jumpForce, smoothTime;

    public Vector2 inputDir;
    public Vector3 lastMoveDir, moveDir, moveAmount, smoothMoveVelocity;
    public float gravity = 30f;
    public bool isSprinting => canSprint && Input.GetKey(KeyCode.LeftShift);
    public bool isCrouched;
    
    [Header("Camera Settings")] 
    

    public GameObject PlayerCam;
    private float lookSpeedX = 2.0f;
    private float lookSpeedY = 2.0f;
    private float upperLookLimit = 80f;
    private float lowerLookLimit = 80f;
    private float rotationX;

    [Header("Head Bob Settings")] 
    public float walkBobSpeed;
    public float walkBobAmount;
    public float sprintBobSpeed;
    public float sprintBobAmount;
    public float crouchBobSpeed;
    public float crouchBobAmount;
    public float timer;
    private float defaultYPos = 0;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        PlayerCam = transform.Find("Cam").gameObject;
        defaultYPos = PlayerCam.transform.localPosition.y;


    }

    #region Unity Callbacks

    void Update()
    {
        if (canMove)
        {
            MovementInput();
            ApplyMovement();
            if (headBobEnabled)
            {
                HeadBobVertical();
            }
        }
            
        if (canMouseLook) 
            HandleMouseLook();


    }

    private void HeadBobVertical()
    {
        if (!characterController.isGrounded)
            return;
        if (Mathf.Abs(moveDir.x) > 0.1f || Mathf.Abs(moveDir.z) > 0.1f)
        {
            timer += Time.deltaTime * (isCrouched ? crouchBobSpeed : isSprinting ? sprintBobSpeed : walkBobSpeed);
            PlayerCam.transform.localPosition = new Vector3(PlayerCam.transform.localPosition.x,
                defaultYPos + Mathf.Sin(timer) *
                (isCrouched ? crouchBobAmount : isSprinting ? sprintBobAmount : walkBobAmount),
                PlayerCam.transform.localPosition.z);
        }
    }

    #endregion

    

    private void MovementInput()
    {
        inputDir = new Vector2((isSprinting ? sprintSpeed : walkSpeed) * Input.GetAxis("Vertical"), (isSprinting ? sprintSpeed : walkSpeed) * Input.GetAxis("Horizontal"));
        float moveDirY = inputDir.y;
        moveDir = transform.TransformDirection(Vector3.forward) * inputDir.x + transform.TransformDirection(Vector3.right) * inputDir.y;
        moveDir.y = transform.position.y;
    }
    private void HandleMouseLook()
    {
        rotationX -= Input.GetAxis("Mouse Y") * lookSpeedY;
        rotationX = Mathf.Clamp(rotationX, -upperLookLimit, lowerLookLimit);
        PlayerCam.transform.localRotation= Quaternion.Euler(rotationX, 0, 0 );
        
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeedX, 0);
    }
    private void ApplyMovement()
    {
        if (!characterController.isGrounded) 
            moveDir.y -= gravity * Time.deltaTime;

        characterController.Move(moveDir * Time.deltaTime);
    }
}
