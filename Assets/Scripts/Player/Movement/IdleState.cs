using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.SceneManagement;


public class IdleState : MovementBaseState
{
    private Animator _animator;
    private Rigidbody _rigidbody;
    public override void EnterState(MovementStateManager movementStateManager)
    {
        
        _rigidbody = movementStateManager.rigidbody;
        _animator = movementStateManager.animator;

        _rigidbody.isKinematic = false;
        _animator.Play("Idle");
     
    }

    public override void UpdateState(MovementStateManager movementStateManager)
    {
        if (Input.GetKeyDown(KeyCode.Space) && movementStateManager.grounded)
        {
            movementStateManager.SwitchState(movementStateManager.jumpingState);
        }

        if (!(movementStateManager.grounded || Input.GetKeyDown(KeyCode.Space) ) && _rigidbody.velocity.y<-6)
        {
            movementStateManager.SwitchState(movementStateManager.fallingState);
        }
        
        if (movementStateManager.isDead)
        {
            movementStateManager.SwitchState(movementStateManager.deathState);
            return;
        }
        
        if(movementStateManager._grapplingGun.isGrappled)
            movementStateManager.SwitchState(movementStateManager.grappleState);
     
        
        
        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)))
            movementStateManager.SwitchState(movementStateManager.walkState);
        
      
            
            
    }

    public override void FixedUpdateState(MovementStateManager movementStateManager)
    {
       
    }

    public override void OnCollisionEnter(MovementStateManager movementStateManager,Collision collision)
    {
        
    }

    
    
    
}
