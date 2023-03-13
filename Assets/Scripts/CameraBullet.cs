using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBullet : MonoBehaviour
{
    private Vector3 player;
    [SerializeField] private float speed;
    private Vector3 reference;
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform.position;
        Destroy(gameObject,1f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, player, ref reference, speed);
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