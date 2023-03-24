using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovementStateManager : MonoBehaviour
{
    public Camera mainCamera;
    [Header("WalkSettings")] public float walkSpeed;
    [Header("RunSettings")] public float runSpeed;
    [Header("SwingSettings")] public float swingSpeed=10;
  
    
    [Header("Sounds")]
    public AudioSource stepSound;

    public AudioSource runSound;
    
    [Header("JumpSettings")] public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    
    [Header("RotationSettings")]
    public float RotationSmoothTime = 0.12f;
    
    [Header("Ground Check")] public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;

    [Header("SlopeSettings")] public float slopeForce;
    public float slopeForceRayLength;
    [Header("Sensitivity")] public float sensitivity = 1f;

    [Header("Cinemachine")]
    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    public GameObject CinemachineCameraTarget;
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;
    [Header("Camera Clamp")]
    public float TopClamp = 70.0f;
    public float BottomClamp = -30.0f;
    public float CameraAngleOverride = 0.0f;
    [Header("Death")] public GameObject deathScreen;
    public bool isDead;

   
    public MovementBaseState currentState;
    public Animator animator;
    public IdleState idleState = new IdleState();
    public WalkState walkState = new WalkState();
    public RunState runState = new RunState();
    public DeathState deathState = new DeathState();
    public GrappleState grappleState = new GrappleState();
    public FallingState fallingState = new FallingState();
    
    [HideInInspector]
    public Rigidbody rigidbody;
    [HideInInspector]
    public GrapplingGun _grapplingGun;
    
    float _rotationVelocity;
    private float _targetRotation;



    void Start()
    {
        stepSound.enabled = false;
        runSound.enabled = false;
        deathScreen.SetActive(false);
        _grapplingGun = GetComponentInChildren<GrapplingGun>();

        rigidbody = GetComponent<Rigidbody>();
        currentState = idleState;
        

        currentState.EnterState(this);
        
        _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
    }

 
    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * .5f +.2f, whatIsGround);
        
      
        currentState.UpdateState(this);
        
        
    }

    private void FixedUpdate()
    {
        currentState.FixedUpdateState(this);
    }

    private void LateUpdate()
    {
        CameraRotation();
    }
    private void CameraRotation()
    {
        // if there is an input and camera position is not fixed
        if(Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            //Don't multiply mouse input by Time.deltaTime;
            float deltaTimeMultiplier =  1.0f;

            _cinemachineTargetYaw += Input.GetAxis("Mouse X") * deltaTimeMultiplier * sensitivity;
            _cinemachineTargetPitch +=  Input.GetAxis("Mouse Y")* deltaTimeMultiplier * sensitivity;

    
        }

        // clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        // Cinemachine will follow this target
        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
            _cinemachineTargetYaw, 0.0f);
    }

    public void SetSensitivity(float newSensitivity)
    {
        sensitivity = newSensitivity;
    }
    

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
    
    public void SwitchState(MovementBaseState movementBaseState)
    {
        currentState = movementBaseState;
        movementBaseState.EnterState(this);
    }

    public void OnCollisionEnter(Collision collision)
    {
        currentState.OnCollisionEnter(this,collision);
    }
}
