using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Mail : MonoBehaviour
{
    [SerializeField]
  private GameObject mail;

  [SerializeField] private TextMeshProUGUI textInfo;

  private bool isActive = true;

  public static bool isMailRead = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerStay(Collider collision)
    {
        if (!mail.activeInHierarchy)
            if(isMailRead)
                textInfo.text = "Go to teleporter room";
            else
            {
                textInfo.text = "Press E to read message";       
            }
         
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.E))
            {
                mail.SetActive(true);
                isMailRead = true;
            }
            
            if(Input.GetKey(KeyCode.Return))
                mail.SetActive(false);

        }
    }

    public void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isMailRead)
            textInfo.text = "Check YOUR COMPUTER";
    }
}
