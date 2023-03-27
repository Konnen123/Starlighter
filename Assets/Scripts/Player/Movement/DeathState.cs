using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class DeathState : MovementBaseState
{
    private Animator _animator;

    public override void EnterState(MovementStateManager movementStateManager)
    {
        if (movementStateManager.deathScreen.activeInHierarchy)
        {
            movementStateManager.SwitchState(movementStateManager.idleState);
            return;
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
      AudioManager.Instance.playSound(AudioManager.Instance.death);

        _animator = movementStateManager.animator;
        movementStateManager.GetComponent<ThirdPersonShooterController>().rifle.gameObject.SetActive(false);
        movementStateManager.GetComponent<ThirdPersonShooterController>().crossHair.SetActive(false);
        movementStateManager.GetComponent<ThirdPersonShooterController>().enabled = false;
   
        movementStateManager.mainCamera.GetComponent<ThirdPersonCamera>().enabled = false;
        movementStateManager.mainCamera.transform.parent.GetChild(2).gameObject.SetActive(false);//set false aim camera
        movementStateManager.GetComponent<RigBuilder>().enabled = false;

        movementStateManager.StartCoroutine(beginDeath(movementStateManager));
    }

    public override void UpdateState(MovementStateManager movementStateManager)
    {
    
    }

    public override void FixedUpdateState(MovementStateManager movementBaseState)
    {
    }

    public override void OnCollisionEnter(MovementStateManager movementStateManager, Collision collision)
    {
      
    }

    IEnumerator beginDeath(MovementStateManager movementStateManager)
    {

        _animator.Play("Death");
        yield return new WaitForSeconds(4f);
        GameObject[] allAudioOutputs = GameObject.FindGameObjectsWithTag("Audio");
        foreach (var audio in allAudioOutputs)
        {
            if(audio.name =="HeartBeat") 
                audio.SetActive(false);
            audio.GetComponent<AudioSource>().enabled=false;
        }
        movementStateManager.deathScreen.SetActive(true);
        yield return new WaitForSeconds(2f);
        movementStateManager.deathScreen.transform.GetChild(1).gameObject.SetActive(true);
        movementStateManager.SwitchState(movementStateManager.idleState);

    }
}
