using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipCollider : MonoBehaviour
{
    [SerializeField] private string tipText;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TipText.Instance.SetText(tipText);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
            TipText.Instance.SetText("");
    }

    private void OnDestroy()
    {
        TipText.Instance.SetText("");
    }
}
