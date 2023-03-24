using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingState : MovementBaseState
{
    
    private Animator _animator;
    private Rigidbody _rigidbody;
    public override void EnterState(MovementStateManager movementStateManager)
    {
        movementStateManager.stepSound.enabled = false;
        movementStateManager.runSound.enabled = false;
        _animator = movementStateManager.animator;
        _rigidbody = movementStateManager.rigidbody;
        
        _animator.Play("Falling");
    }

    public override void UpdateState(MovementStateManager movementStateManager)
    {
 
      if(movementStateManager.grounded)
          movementStateManager.SwitchState(movementStateManager.idleState);
      
      if (movementStateManager.isDead)
      {
          movementStateManager.SwitchState(movementStateManager.deathState);
          return;
      }
      if (movementStateManager._grapplingGun.isGrappled)
      {

          movementStateManager.SwitchState(movementStateManager.grappleState);
      }
    }

    public override void FixedUpdateState(MovementStateManager movementStateManager)
    {
        Movement(movementStateManager);
    }

    public override void OnCollisionEnter(MovementStateManager movementStateManager, Collision collision)
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
        
        
        _rigidbody.MovePosition(movementStateManager.transform.position + moveDirection * movementStateManager.walkSpeed * Time.fixedDeltaTime);

    }
}
