using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBullet : MonoBehaviour
{
    private Transform player;
    private Vector3 playerShootingPosition;
    [SerializeField] private float speed;
    private Vector3 reference;

    private Rigidbody rigidbody;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("Player").transform;
        playerShootingPosition = player.GetChild(player.childCount-1).position;
        Destroy(gameObject,1f);
    }

    // Update is called once per frame
    void Update()
    {
       transform.position = Vector3.SmoothDamp(transform.position, playerShootingPosition, ref reference, speed);
    if(Mathf.Abs(transform.position.x-playerShootingPosition.x)<.1f)
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HealthManager.Instance.TakeDamage(20);
            Destroy(gameObject);
        }
        if (other.CompareTag("Wall")||  other.CompareTag("Ground"))
        {
    
            Destroy(gameObject);
        }
  
    }
}