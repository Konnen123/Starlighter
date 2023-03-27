using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JumpingState : MovementBaseState
{
    private Animator _animator;
    private Rigidbody _rigidbody;
    private bool isJumping=true;

    private float speed;
    public override void EnterState(MovementStateManager movementStateManager)
    {

        speed = movementStateManager.walkSpeed;
        if (movementStateManager.runSound.enabled)
            speed = movementStateManager.runSpeed;
        
        movementStateManager.runSound.enabled = false;
        movementStateManager.stepSound.enabled = false;
        
         _animator = movementStateManager.animator; 
         _rigidbody = movementStateManager.rigidbody; 
         movementStateManager.StartCoroutine(Jump(movementStateManager));
  
        
    }

    public override void UpdateState(MovementStateManager movementStateManager)
    {
        if (movementStateManager.grounded && !isJumping)
        {
            movementStateManager.SwitchState(movementStateManager.idleState);
        }
        if (movementStateManager.grounded && !isJumping && speed==movementStateManager.runSpeed)
        {
            movementStateManager.SwitchState(movementStateManager.runState);
        }
        if (movementStateManager.isDead)
        {
            movementStateManager.SwitchState(movementStateManager.deathState);
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

      

        
        _rigidbody.MovePosition(movementStateManager.transform.position + moveDirection * speed * Time.fixedDeltaTime);
        
        
    }
    
    IEnumerator Jump(MovementStateManager movementStateManager)
    {
        isJumping = true;
        if (SceneManager.GetActiveScene().name == "Intro")
        {
            movementStateManager.SwitchState(movementStateManager.idleState);
            yield break;
        }
        
        _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z);
        _rigidbody.AddForce(movementStateManager.transform.up * movementStateManager.jumpForce,ForceMode.Impulse);
        yield return null;
        
        _animator.Play("Jump");
        
        yield return null;
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);
        isJumping = false;

    }

}

