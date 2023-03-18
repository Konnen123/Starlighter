using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class MovingTiles : MonoBehaviour
{
    [SerializeField] private Vector3 finalPosition;
    [SerializeField] private float time;
    [SerializeField] private float idleTime;

    private Vector3 currentVelocity;
    private Vector3 currentPosition;
    private float currentTime;

    private bool finishLine;

    private bool isOnTile;

    private GameObject player;

    private Vector3 moveDirection;

    private Vector3 offset;

    private Camera camera;
    
    // Start is called before the first frame update
    void Start()
    {
        
     camera = Camera.main;
        currentPosition = transform.localPosition;

    }

    // Update is called once per frame
    void Update()
    {
        if (!finishLine)
        {
           moveDirection = Vector3.SmoothDamp(transform.localPosition, finalPosition, ref currentVelocity, time);
                
            transform.localPosition = moveDirection;

            if (Mathf.Abs(transform.localPosition.x - finalPosition.x) < .1f)
            {
                currentTime += Time.deltaTime;
                if (currentTime >= idleTime)
                {
                    finishLine = true;
                    currentTime = 0;

                }
            }
        }

        if (finishLine)
        {
            moveDirection =
                Vector3.SmoothDamp(transform.localPosition, currentPosition, ref currentVelocity, time);
            transform.localPosition = moveDirection;
            if (Mathf.Abs(transform.localPosition.x - currentPosition.x) < .1f)
            {
                currentTime += Time.deltaTime;
                if (currentTime >= idleTime)
                {
                    finishLine = false;
                    currentTime = 0;

                }
            }
        }

        
    }

  

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            camera.GetComponent<CinemachineBrain>().m_UpdateMethod = CinemachineBrain.UpdateMethod.LateUpdate;
            player = other.gameObject;
            player.transform.SetParent(transform,true);
            

           // offset = player.transform.position - transform.position;
            isOnTile = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
    
        if (other.gameObject.CompareTag("Player"))
        {
            camera.GetComponent<CinemachineBrain>().m_UpdateMethod = CinemachineBrain.UpdateMethod.FixedUpdate;
            player.transform.SetParent(null);
            isOnTile = false;
        }
    }
}

   

