using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WalkState : MovementBaseState
{
    private Animator _animator;
    private Rigidbody _rigidbody;
    private bool isJumping;
    private float currentJumpTime;
    public override void EnterState(MovementStateManager movementStateManager)
    {
        movementStateManager.stepSound.enabled = true;
        _animator = movementStateManager.animator;
        _rigidbody = movementStateManager.rigidbody;
    }

    public override void UpdateState(MovementStateManager movementStateManager)
    {
        if (!(movementStateManager.grounded || Input.GetKeyDown(KeyCode.Space)) && !isJumping && _rigidbody.velocity.y<-3)
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
            movementStateManager.stepSound.enabled = false;
            movementStateManager.SwitchState(movementStateManager.runState);
        }

        if (movementStateManager._grapplingGun.isGrappled)
        {
            movementStateManager.stepSound.enabled = false;
            movementStateManager.SwitchState(movementStateManager.grappleState);
        }
            
        
        if (Input.GetKeyDown(KeyCode.Space) && movementStateManager.grounded)
        {
            movementStateManager.stepSound.enabled = false;
            Jump(movementStateManager);
        }

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
        
        
        if (velocity.magnitude < .1f && !isJumping)
        {
            movementStateManager.stepSound.enabled = false;
            movementStateManager.SwitchState(movementStateManager.idleState);
            return;
        }
        
        _rigidbody.MovePosition(movementStateManager.transform.position + moveDirection * movementStateManager.walkSpeed* Time.fixedDeltaTime);
        
        
        if (!isJumping && movementStateManager.grounded)
            _animator.Play("Walk");
        
    }


    void Jump(MovementStateManager movementStateManager)
    {
        if (SceneManager.GetActiveScene().name == "Intro")
        {
            movementStateManager.SwitchState(movementStateManager.walkState);
            return;
        }
        isJumping = true;
       
        _animator.Play("Jump");
        _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z);
        _rigidbody.AddForce(movementStateManager.transform.up * movementStateManager.jumpForce,ForceMode.Impulse);
        
 

    }
    
}
