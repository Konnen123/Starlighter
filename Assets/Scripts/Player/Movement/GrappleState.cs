using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleState : MovementBaseState
{
    private Animator _animator;
    private Rigidbody _rigidbody;
    private CapsuleCollider _collider;
    public override void EnterState(MovementStateManager movementStateManager)
    {
        _animator = movementStateManager.animator;
        _rigidbody = movementStateManager.rigidbody;
        _collider = movementStateManager.GetComponent<CapsuleCollider>();
        _collider.center= new Vector3(0, 2.13f, 0);
        
      
       
        
        _animator.Play("Swing");
    }

    public override void UpdateState(MovementStateManager movementStateManager)
    {
        if (movementStateManager.isDead)
        {
            _collider.center = new Vector3(0, 0.9329002f, 0);
            movementStateManager.SwitchState(movementStateManager.deathState);
            return;
        }
        
        if (!movementStateManager._grapplingGun.isGrappled)
        {
            _collider.center = new Vector3(0, 0.9329002f, 0);
            movementStateManager.SwitchState(movementStateManager.fallingState);
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

        moveDirection = moveDirection.normalized * Time.fixedDeltaTime * movementStateManager.swingSpeed;

        _rigidbody.MovePosition(movementStateManager.transform.position + moveDirection);
    }

}
