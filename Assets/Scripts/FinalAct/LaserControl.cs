using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserControl : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float coolDown;
    private MeshRenderer _renderer;
    private CapsuleCollider _collider;
    private Animator _animator;
    void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        _collider = GetComponent<CapsuleCollider>();
        _animator = GetComponent<Animator>();
        
        visualAndCollide(false);
        StartCoroutine(moveLaser());

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void visualAndCollide(bool active)
    {
        _renderer.enabled = active;
        _collider.enabled = active;
        _animator.enabled = active;
    }

    IEnumerator moveLaser()
    {
        visualAndCollide(true);
        yield return new WaitForSeconds(4f);
        visualAndCollide(false);
        
        yield return new WaitForSeconds(coolDown);
        StartCoroutine(moveLaser());


    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            HealthManager.Instance.TakeDamage(30);
    }
}