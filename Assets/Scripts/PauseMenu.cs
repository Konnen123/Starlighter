using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private bool isPaused = false;
    private GameObject backGround;
    private ThirdPersonShooterController shooterController;
    private MovementStateManager stateManager;

    private Camera camera;

    private void Start()
    {
        camera=Camera.main;
        backGround = transform.GetChild(0).gameObject;
        backGround.SetActive(false);
        shooterController = transform.parent.GetComponent<ThirdPersonShooterController>();
        stateManager = transform.parent.GetComponent<MovementStateManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            camera.GetComponent<ThirdPersonCamera>().enabled = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
            stateManager.enabled = false; 
            if(shooterController!=null)
            shooterController.enabled = false; 
            backGround.SetActive(true);
                
            
        }
        
    }

    public void Unpause()
    {
        camera.GetComponent<ThirdPersonCamera>().enabled = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        stateManager.enabled = true;
        if(shooterController!=null)
        shooterController.enabled = true;
        backGround.SetActive(false);
        isPaused = false;
    }

    public void Menu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
