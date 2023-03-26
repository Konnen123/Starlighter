using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RunState : MovementBaseState
{
    private Animator _animator;
    private Rigidbody _rigidbody;


    private Vector3 moveDirection;
    
    RaycastHit slopeHit;
    public override void EnterState(MovementStateManager movementStateManager)
    {
        if (SceneManager.GetActiveScene().name == "Intro")
        {
            
            movementStateManager.SwitchState(movementStateManager.walkState);
            return;
        }
        movementStateManager.stepSound.enabled = false;
        movementStateManager.runSound.enabled = true;
        _rigidbody = movementStateManager.rigidbody;
        _animator = movementStateManager.animator;
        
    }

    public override void UpdateState(MovementStateManager movementStateManager)
    {
       
        if (!(movementStateManager.grounded || Input.GetKeyDown(KeyCode.Space))  && _rigidbody.velocity.y<-6)
        {
      
            movementStateManager.runSound.enabled = false;
            movementStateManager.SwitchState(movementStateManager.fallingState);
        }
        
        if (movementStateManager.isDead)
        {
            movementStateManager.runSound.enabled = false;
            movementStateManager.SwitchState(movementStateManager.deathState);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && movementStateManager.grounded)
        {
            movementStateManager.SwitchState(movementStateManager.jumpingState);
        }

        if (movementStateManager._grapplingGun.isGrappled)
        {
            movementStateManager.runSound.enabled = false;
            movementStateManager.SwitchState(movementStateManager.grappleState);
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
        
        
        if (velocity.magnitude < .1f && movementStateManager.grounded)
        {
            movementStateManager.runSound.enabled = false;
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
         _animator.Play("Run");
        
      //    _rigidbody.MovePosition(movementStateManager.transform.position +  Vector3.down * movementStateManager.playerHeight / 2 * movementStateManager.slopeForce  * Time.fixedDeltaTime);
      //   movementStateManager.transform.position += Vector3.down * movementStateManager.playerHeight / 2 * movementStateManager.slopeForce  * Time.fixedDeltaTime;
       
       
           
           
    }

    IEnumerator stop(MovementStateManager movementStateManager)
    {
        
        _rigidbody.velocity=Vector3.zero;
        yield return new WaitForSeconds(.2f);
        movementStateManager.runSound.enabled = false;
       
        movementStateManager.SwitchState(movementStateManager.idleState);
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
