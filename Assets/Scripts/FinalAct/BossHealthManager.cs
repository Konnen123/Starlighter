using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthManager : MonoBehaviour
{
    public static BossHealthManager Instance { get; private set; }
    [SerializeField] float maxHealth;
    [SerializeField] private GameObject boss,bossEyes,bossAttacks,cutScene;
    [SerializeField] private Material blackSkybox;
    
    [SerializeField] private Image healthBar;
    [SerializeField] private AudioSource heartBeat;
    private bool weakSpotFound;

    public bool isHalfHp;
    public bool isLowHp;

    private float currentHealth;

    private bool changeSky;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        currentHealth = maxHealth;
        cutScene.SetActive(false);
       
    }

    private void Update()
    {
        if(changeSky)
            RenderSettings.skybox.Lerp( RenderSettings.skybox,blackSkybox,.01f);
    }

    public void TakeDamage(float damage)
    {
     
            
        if(currentHealth<=0)
            return;

        if (damage > 50 && !weakSpotFound)
            StartCoroutine(tipWeakSpot());

        currentHealth -= damage;
        if (currentHealth <= maxHealth / 2)
            isHalfHp = true;
        if (currentHealth <= maxHealth / 3)
        {
            isHalfHp = false;
            isLowHp = true;
        }
        healthBar.fillAmount = currentHealth/maxHealth;
        if (currentHealth <= 0)
        {
            Debug.Log("You won");
            cutScene.SetActive(true);
            Destroy(bossEyes);
            Destroy(bossAttacks);
            changeSky = true;
            Destroy(heartBeat);

        }
          
    }

    IEnumerator tipWeakSpot()
    {
        weakSpotFound = true;
        TipText.Instance.SetText("NOT MY EYES!");
        yield return new WaitForSeconds(2f);
        TipText.Instance.SetText("");
    }
    
}
