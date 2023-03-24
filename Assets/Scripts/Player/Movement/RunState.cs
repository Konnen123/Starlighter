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
        movementStateManager.runSound.enabled = true;
        _rigidbody = movementStateManager.rigidbody;
        _animator = movementStateManager.animator;
        
    }

    public override void UpdateState(MovementStateManager movementStateManager)
    {
       
        if (!(movementStateManager.grounded || Input.GetKeyDown(KeyCode.Space)) && !isJumping && _rigidbody.velocity.y<-1)
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
            movementStateManager.runSound.enabled = false;
            Jump(movementStateManager);
        }

        if (movementStateManager._grapplingGun.isGrappled)
        {
            movementStateManager.runSound.enabled = false;
            movementStateManager.SwitchState(movementStateManager.grappleState);
        }
        
        if (isJumping)
        {

            if (currentJumpTime >= movementStateManager.jumpCooldown)
            {
                movementStateManager.runSound.enabled = false;
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
            movementStateManager.runSound.enabled = false;
            movementStateManager.SwitchState(movementStateManager.idleState);
            return;
        }
        
       _rigidbody.MovePosition(movementStateManager.transform.position +moveDirection);
       

       
        if(!isJumping)
        _animator.Play("Run");
        

       

   //    if (OnSlope(movementStateManager))
      //    _rigidbody.MovePosition(movementStateManager.transform.position +  Vector3.down * movementStateManager.playerHeight / 2 * movementStateManager.slopeForce  * Time.fixedDeltaTime);
      //   movementStateManager.transform.position += Vector3.down * movementStateManager.playerHeight / 2 * movementStateManager.slopeForce  * Time.fixedDeltaTime;
       
       
           
           
    }

    IEnumerator stop(MovementStateManager movementStateManager)
    {
        
        _rigidbody.velocity=Vector3.zero;
        yield return new WaitForSeconds(.2f);
        movementStateManager.runSound.enabled = false;
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
    
    void Jump(MovementStateManager movementStateManager)
    {
        isJumping = true;
       
        _animator.Play("Jump");
        _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z);
        _rigidbody.AddForce(movementStateManager.transform.up * movementStateManager.jumpForce,ForceMode.Impulse);

    }
}
