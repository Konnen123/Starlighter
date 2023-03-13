using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GunMovement : MonoBehaviour
{
    private Camera mainCamera;


    private void Start()
    {
        mainCamera = Camera.main;
        
    }

    private void FixedUpdate()
    {
        Vector3 difference = mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        difference.Normalize();
        float rotationY = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        
        transform.rotation=Quaternion.Euler(rotationY,0,0);
    }
}
