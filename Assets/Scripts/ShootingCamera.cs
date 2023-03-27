using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class ShootingCamera : MonoBehaviour
{
    [SerializeField]private GameObject bullet,player;
    [SerializeField] private float shootRate,health=50,minDistance;
    [SerializeField] private LayerMask aimColliderLayerMask;
    [SerializeField] private bool isRotatable;

    private bool isInRange;
    private float currentTime;
    private Transform projectileSpawner;

    private MovementStateManager movementStateManager;
    // Start is called before the first frame update
    void Start()
    {
        currentTime = shootRate;
        projectileSpawner = transform.GetChild(0);
        movementStateManager = player.GetComponent<MovementStateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (movementStateManager.isDead) 
            return;
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if(isRotatable)
            transform.LookAt(player.transform);
        else
            projectileSpawner.LookAt(player.transform);
        if (distance<minDistance && currentTime>=shootRate)
        {
            Ray ray = new Ray(projectileSpawner.position, projectileSpawner.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit,distance,aimColliderLayerMask))
                if(!hit.collider.CompareTag("Player"))
                    return;
        
            
            Transform playerPos = player.transform;
            GameObject cameraBullet = Instantiate(bullet,projectileSpawner,false);
            cameraBullet.transform.localPosition = Vector3.zero;
            cameraBullet.transform.SetParent(null);

            currentTime = 0;

        }

        if (currentTime < shootRate)
            currentTime += Time.deltaTime;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health<=0)
            Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position,minDistance);
        Gizmos.color=Color.red;
    }
}
