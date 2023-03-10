using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("References")] public Transform orientation;
    public Transform player;
    public Transform playerObject;
    public Rigidbody rigidBody;
    [SerializeField] private float min;
   

    public float rotationSpeed;
    private bool rotateOnMove=true;
   
    void Start()
    {
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
        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (inputDir.magnitude > Vector3.one.magnitude* min && rotateOnMove)
            playerObject.forward = Vector3.Slerp(playerObject.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
    }

    public void SetRotateOnMove(bool newRotateOnMove)
    {
        rotateOnMove = newRotateOnMove;
    }
    
}
