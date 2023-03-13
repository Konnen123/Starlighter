using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleporterRoom : MonoBehaviour
{
    
    [SerializeField] private TextMeshProUGUI textInfo;
    public void OnTriggerStay(Collider collision)
    {
        
        if (collision.gameObject.CompareTag("Player") && Mail.isMailRead)
        {
            textInfo.text = "Press E to teleport";
            if (Input.GetKey(KeyCode.E))
            {
                textInfo.text = "";
                int sceneIndex = SceneManager.GetActiveScene().buildIndex;
           
                if (sceneIndex > PlayerPrefs.GetInt("sceneIndex"))
                {
                    PlayerPrefs.SetInt("sceneIndex",sceneIndex);
                    PlayerPrefs.Save();
                }
                SceneManager.LoadScene("Act1");
            }
        }
    }
    
}
