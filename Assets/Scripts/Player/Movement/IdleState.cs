using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.SceneManagement;


public class IdleState : MovementBaseState
{
    private Animator _animator;
    private Rigidbody _rigidbody;
    private bool isJumping;
    private float currentJumpTime;
    public override void EnterState(MovementStateManager movementStateManager)
    {
        
        _rigidbody = movementStateManager.rigidbody;
        _animator = movementStateManager.animator;

        _rigidbody.isKinematic = false;
        _animator.Play("Idle");
     
    }

    public override void UpdateState(MovementStateManager movementStateManager)
    {
        
        if (!(movementStateManager.grounded || Input.GetKeyDown(KeyCode.Space)) && !isJumping)
        {
            movementStateManager.SwitchState(movementStateManager.fallingState);
        }
        
        if (movementStateManager.isDead)
        {
            movementStateManager.SwitchState(movementStateManager.deathState);
            return;
        }

        if (isJumping)
        {

            if (currentJumpTime >= movementStateManager.jumpCooldown)
            {
                _animator.Play("Idle");
                isJumping = false;
                currentJumpTime = 0;
            }
            else
            {
                currentJumpTime += Time.deltaTime;
            }
            
        }
            if(movementStateManager._grapplingGun.isGrappled)
            movementStateManager.SwitchState(movementStateManager.grappleState);
     
        
        
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) ||
            Input.GetKey(KeyCode.A))
            movementStateManager.SwitchState(movementStateManager.walkState);
        
        if (Input.GetKeyDown(KeyCode.Space) && movementStateManager.grounded)
        {
            Jump(movementStateManager);
        }
            
            
    }

    public override void FixedUpdateState(MovementStateManager movementStateManager)
    {
       
    }

    public override void OnCollisionEnter(MovementStateManager movementStateManager,Collision collision)
    {
        
    }

    
    
    
    void Jump(MovementStateManager movementStateManager)
    {
        if (SceneManager.GetActiveScene().name == "Intro")
        {
            movementStateManager.SwitchState(movementStateManager.idleState);
            return;
        }
        isJumping = true;
       
        _animator.Play("Jump");
        _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z);
        _rigidbody.AddForce(movementStateManager.transform.up * movementStateManager.jumpForce,ForceMode.Impulse);
        
    
        
    }
}
