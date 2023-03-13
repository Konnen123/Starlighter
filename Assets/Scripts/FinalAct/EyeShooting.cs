using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EyeShooting : MonoBehaviour
{
    [SerializeField] private List<GameObject> bullet;
    [SerializeField] private GameObject player;
    public float shootRate;

    private bool isInRange;
    private float currentTime;
    private Transform projectileSpawner;

    public static int randomNumber;
    // Start is called before the first frame update
    void Start()
    {
        currentTime = 0;
        projectileSpawner = transform;
    }

    // Update is called once per frame
    void Update()
    {


        if (currentTime >= shootRate)
        {

            randomNumber = Random.Range(0,bullet.Count);
            Transform playerPos = player.transform;
            GameObject cameraBullet = Instantiate(bullet[randomNumber], projectileSpawner);
            
            cameraBullet.transform.localPosition = Vector3.zero;
            cameraBullet.transform.SetParent(null);
            cameraBullet.transform.localScale = Vector3.one *2;
            currentTime = 0;

        }

        if (currentTime < shootRate)
                currentTime += Time.deltaTime;
        
    }

  

 
}