using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] Transform checkPointPosition;
    [SerializeField] private CinemachineVirtualCamera aimCamera, explorationCamera;
    private GameObject player;
    private bool checkpoint;
    private void Awake()
    {
        player = transform.parent.gameObject;
    }


    public void GoToMenu()
    {
        LevelTransition.Instance.LoadScene("Menu");
    }

    public void Restart()
    {
        MovementStateManager movementStateManager = player.GetComponent<MovementStateManager>();
        checkpoint = movementStateManager.checkPoint;
        if (!checkpoint)
        {
            LevelTransition.Instance.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }
        
       StartCoroutine(ReanimatePlayer(movementStateManager));
    }

    IEnumerator ReanimatePlayer(MovementStateManager movementStateManager)
    {
        aimCamera.enabled = false;
        explorationCamera.enabled = false;

        yield return new WaitForSeconds(.1f);
        player.transform.position = checkPointPosition.position;
        LevelTransition.Instance.playCircleWipeStart();
        yield return new WaitForSeconds(1f);
        aimCamera.enabled = true;
        explorationCamera.enabled = true;
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        movementStateManager.isDead = false;
        HealthManager.Instance.HealToFull();       
        movementStateManager.SwitchState(movementStateManager.idleState);
 
        
        movementStateManager.GetComponent<ThirdPersonShooterController>().rifle.gameObject.SetActive(true);
        movementStateManager.GetComponent<ThirdPersonShooterController>().crossHair.SetActive(true);
        movementStateManager.GetComponent<ThirdPersonShooterController>().enabled = true;
   
        movementStateManager.mainCamera.GetComponent<ThirdPersonCamera>().enabled = true;
        movementStateManager.mainCamera.transform.parent.GetChild(2).gameObject.SetActive(true);//set false aim camera
        movementStateManager.GetComponent<RigBuilder>().enabled = true;
        
        GameObject[] allAudioOutputs = GameObject.FindGameObjectsWithTag("Audio");
        foreach (var audio in allAudioOutputs)
        {
            if (audio.name == "HeartBeat")
            {
                audio.SetActive(true);
                continue;
            }
            audio.GetComponent<AudioSource>().enabled=true;
        }
       // LevelTransition.Instance.playCircleWipeEnd();
        gameObject.SetActive(false);
    
    }
}
