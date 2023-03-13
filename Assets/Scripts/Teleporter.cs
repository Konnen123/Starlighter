using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private string sceneName;
    private void OnTriggerStay(Collider other)
    {
       
        if (other.gameObject.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.E))
            {
                int sceneIndex = SceneManager.GetActiveScene().buildIndex;
           
                if (sceneIndex > PlayerPrefs.GetInt("sceneIndex"))
                {
                    PlayerPrefs.SetInt("sceneIndex",sceneIndex);
                    PlayerPrefs.Save();
                }
                SceneManager.LoadScene(sceneName);      
            }
          
        }
    }
}
