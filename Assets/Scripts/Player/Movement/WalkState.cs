using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WalkState : MovementBaseState
{
    private Animator _animator;
    private Rigidbody _rigidbody;
    
    RaycastHit slopeHit;
    public override void EnterState(MovementStateManager movementStateManager)
    {
        if(movementStateManager.grounded)
          movementStateManager.stepSound.enabled = true;
        
        _animator = movementStateManager.animator;
        _rigidbody = movementStateManager.rigidbody;
    }

    public override void UpdateState(MovementStateManager movementStateManager)
    {
        if (!(movementStateManager.grounded || Input.GetKeyDown(KeyCode.Space)) && _rigidbody.velocity.y<-6)
        {
            movementStateManager.stepSound.enabled = false;
            movementStateManager.SwitchState(movementStateManager.fallingState);
        }
        
        if (movementStateManager.isDead)
        {
            movementStateManager.stepSound.enabled = false;
            movementStateManager.SwitchState(movementStateManager.deathState);
            return;
        }

        if (Input.GetKey(KeyCode.LeftShift) && movementStateManager.grounded)
        {
            movementStateManager.SwitchState(movementStateManager.runState);
        }

        if (movementStateManager._grapplingGun.isGrappled)
        {
            movementStateManager.stepSound.enabled = false;
            movementStateManager.SwitchState(movementStateManager.grappleState);
        }
            
        
        if (Input.GetKeyDown(KeyCode.Space) && movementStateManager.grounded)
        {
            movementStateManager.SwitchState(movementStateManager.jumpingState);
        }

  
        
   
      
    }
    public override void FixedUpdateState(MovementStateManager movementStateManager)
    {
        Movement(movementStateManager);
    }

    public override void OnCollisionEnter(MovementStateManager movementStateManager,Collision collision)
    {
        
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

        Vector3 moveDirection = horizontal * right + vertical * forward;
        
        moveDirection = moveDirection.normalized * Time.fixedDeltaTime * movementStateManager.walkSpeed;


        if (velocity.magnitude < .1f && movementStateManager.grounded)
        {
            movementStateManager.stepSound.enabled = false;
            movementStateManager.SwitchState(movementStateManager.idleState);
            return;
        }

        Vector3 slopeMovementDirection;
        
        if (OnSlope(movementStateManager))
        {
            slopeMovementDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
            _rigidbody.MovePosition(movementStateManager.transform.position + slopeMovementDirection);
        }
        else
            _rigidbody.MovePosition(movementStateManager.transform.position +moveDirection);
        
        
        if(movementStateManager.grounded)
            _animator.Play("Walk");
        
    }

    private bool OnSlope(MovementStateManager movementStateManager)
    {
        if (!movementStateManager.grounded)
            return false;

     
        
        if(Physics.Raycast(movementStateManager.transform.position,Vector3.down,out slopeHit,movementStateManager.playerHeight/2 +.5f))
            if (slopeHit.normal != Vector3.up)
                return true;
        return false;
    }
 
    
}
