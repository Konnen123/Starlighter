using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("References")] public Transform orientation;
    public Transform player;
    public Transform playerObject;
    [SerializeField] private float min;
   

    public float rotationSpeed;
    private bool rotateOnMove=true;

    private float rotationVelocity;
    private float _targetRotation;
    private Vector3 inputDir;

    private CinemachineBrain cameraBrain;

    private Vector3 currentForward;
    void Start()
    {
        cameraBrain = GetComponent<CinemachineBrain>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    void Update()
    {
        //rotate orientation
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;
        
        //rotate player object
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
         inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;


        

     if (inputDir.magnitude > Vector3.one.magnitude * min && rotateOnMove)
         playerObject.forward =Vector3.Slerp(playerObject.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
  
    }

    private void FixedUpdate()
    {
       
    }

    public void SetRotateOnMove(bool newRotateOnMove)
    {
        rotateOnMove = newRotateOnMove;
    }
    
}