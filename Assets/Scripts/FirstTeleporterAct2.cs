using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstTeleporterAct2 : MonoBehaviour
{
    [SerializeField] private Transform pointToTeleport;
    [SerializeField] private Transform player;
    private bool isInRange;


    private void Update()
    {
        if (isInRange&& Input.GetKeyDown(KeyCode.E))
        {
            player.transform.position = pointToTeleport.position;
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
}
