using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class FirstTeleporterAct2 : MonoBehaviour
{
    [SerializeField] private Transform pointToTeleport;
    [SerializeField] private Transform player;
    private bool isInRange;
    private bool isPressed;

    [SerializeField] private CinemachineVirtualCamera explorationCamera, aimCamera;

    private void Start()
    {
 
    }

    private void Update()
    {
        if (isInRange&& Input.GetKeyDown(KeyCode.E)&& !isPressed)
        {
            isPressed = true;
            StartCoroutine(TeleportPlayer());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            isInRange = true;

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            isInRange = false;

    }

    IEnumerator TeleportPlayer()
    {
        aimCamera.enabled = false;
        explorationCamera.enabled = false;
        yield return null;
        player.transform.position = pointToTeleport.position;
        player.transform.rotation = pointToTeleport.rotation;

        aimCamera.enabled = true;
        explorationCamera.enabled = true;
        
        TipText.Instance.SetText("");

        transform.parent.parent.gameObject.SetActive(false);
    }
}
