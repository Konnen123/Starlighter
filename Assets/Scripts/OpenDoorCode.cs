using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoorCode : MonoBehaviour
{
    [SerializeField] private GameObject leftDoor,rightDoor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(KeyManager.Instance.GetKeyCount()<=0)
                return;
            TipText.Instance.SetText("Press E to enter key");

            if (Input.GetKey(KeyCode.E))
            {
                KeyManager.Instance.UseKey();
                leftDoor.GetComponent<Animator>().enabled = true;
                rightDoor.GetComponent<Animator>().enabled = true;

                leftDoor.GetComponent<Collider>().enabled = false;
                rightDoor.GetComponent<Collider>().enabled = false;
                rightDoor.GetComponentInParent<Collider>().enabled = false;
                
                TipText.Instance.SetText("");
                Destroy(this);
            }
            
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
            TipText.Instance.SetText("");
    }
}
