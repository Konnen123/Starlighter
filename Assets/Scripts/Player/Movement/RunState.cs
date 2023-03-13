using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RunState : MovementBaseState
{
    private Animator _animator;
    private Rigidbody _rigidbody;
    private bool isJumping;
    private float currentJumpTime;

    private Vector3 moveDirection;
    public override void EnterState(MovementStateManager movementStateManager)
    {
        if (SceneManager.GetActiveScene().name == "Intro")
        {
            movementStateManager.SwitchState(movementStateManager.walkState);
            return;
        }
        _rigidbody = movementStateManager.rigidbody;
        _animator = movementStateManager.animator;
        
    }

    public override void UpdateState(MovementStateManager movementStateManager)
    {
        
        if (movementStateManager.isDead)
        {
            movementStateManager.SwitchState(movementStateManager.deathState);
            return;
        }
        
        if(Input.GetKeyDown(KeyCode.Space)&& movementStateManager.grounded)
            Jump(movementStateManager);
        
          if(movementStateManager._grapplingGun.isGrappled)
            movementStateManager.SwitchState(movementStateManager.grappleState);
        
        if (isJumping)
        {

            if (currentJumpTime >= movementStateManager.jumpCooldown)
            {
                movementStateManager.SwitchState(movementStateManager.idleState);
                isJumping = false;
                currentJumpTime = 0;
            }
            else
            {
                currentJumpTime += Time.deltaTime;
            }
            
        }
     
       
        
    }
    
    public override void FixedUpdateState(MovementStateManager movementStateManager)
    {
        Movement(movementStateManager);
    }

    public override void OnCollisionEnter(MovementStateManager movementStateManager,Collision collision)
    {
    
        if(collision.collider.CompareTag("Wall"))
        {
            movementStateManager.StartCoroutine(stop(movementStateManager));
        }
        
    }

 
    
    void Movement(MovementStateManager movementStateManager)
    {
     
        
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        
        var velocity = new Vector3(horizontal, 0f, vertical).normalized;

        Vector3 forward = movementStateManager.mainCamera.transform.forward;
        Vector3 right = movementStateManager.mainCamera.transform.right;

        forward.y = 0;
        right.y = 0;

        moveDirection = horizontal * right + vertical * forward;

        moveDirection = moveDirection.normalized * Time.fixedDeltaTime * movementStateManager.runSpeed;
        
        
        if (velocity.magnitude < .1f && !isJumping)
        {
            movementStateManager.SwitchState(movementStateManager.idleState);
            return;
        }
        
       _rigidbody.MovePosition(movementStateManager.transform.position +moveDirection);


       //   Vector3 cameraPosition = CurrentDirection(movementStateManager);
        
        if(!isJumping)
        _animator.Play("Run");
        
     //  cameraPosition.y = 0;
     
        
     //  movementStateManager.transform.position += Time.fixedDeltaTime * movementStateManager.runSpeed * cameraPosition;
       

   //    if (OnSlope(movementStateManager))
      //    _rigidbody.MovePosition(movementStateManager.transform.position +  Vector3.down * movementStateManager.playerHeight / 2 * movementStateManager.slopeForce  * Time.fixedDeltaTime);
      //   movementStateManager.transform.position += Vector3.down * movementStateManager.playerHeight / 2 * movementStateManager.slopeForce  * Time.fixedDeltaTime;
       
       
           
           
    }

    IEnumerator stop(MovementStateManager movementStateManager)
    {
        
        _rigidbody.velocity=Vector3.zero;
        yield return new WaitForSeconds(.2f);
        Debug.Log("wall");
        movementStateManager.SwitchState(movementStateManager.idleState);
    }

    private bool OnSlope(MovementStateManager movementStateManager)
    {
        if (isJumping)
            return false;

        RaycastHit hit;
        
        if(Physics.Raycast(movementStateManager.transform.position,Vector3.down,out hit,movementStateManager.playerHeight/2*movementStateManager.slopeForceRayLength))
            if (hit.normal != Vector3.up)
                return true;
        return false;
    }
    
    
    Vector3 CurrentDirection(MovementStateManager movementStateManager)
    {
        Transform cameraTransform = movementStateManager.mainCamera.transform;
        
        if(Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
            return cameraTransform.forward +cameraTransform.right;
        
        if(Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
            return cameraTransform.forward -cameraTransform.right;
        
        if(Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
            return -cameraTransform.forward +cameraTransform.right;
        
        if(Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
            return -cameraTransform.forward -cameraTransform.right;
        
        if (Input.GetKey(KeyCode.S))
            return -cameraTransform.forward;
        
        if (Input.GetKey(KeyCode.A))
            return  -cameraTransform.right;
        
        if (Input.GetKey(KeyCode.D))
            return  cameraTransform.right;

        return cameraTransform.forward;
    }
    void Jump(MovementStateManager movementStateManager)
    {
        isJumping = true;
       
        _animator.Play("Jump");
        _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z);
        _rigidbody.AddForce(movementStateManager.transform.up * movementStateManager.jumpForce,ForceMode.Impulse);

    }
}
