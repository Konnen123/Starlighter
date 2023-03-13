using UnityEngine;

public abstract class MovementBaseState
{
    
  public abstract void EnterState(MovementStateManager movementStateManager);

  public abstract void UpdateState(MovementStateManager movementStateManager);

  public abstract void FixedUpdateState(MovementStateManager movementBaseState);

  public abstract void OnCollisionEnter(MovementStateManager movementStateManager,Collision collision);
}
