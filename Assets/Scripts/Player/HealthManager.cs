using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public static HealthManager Instance { get; private set; }
    private GameObject player;
    [SerializeField] private float maxHealth;
    [SerializeField] private Image healthBar;


    private float currentHealth;

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

        player= GameObject.FindWithTag("Player");
        currentHealth = maxHealth;
        healthBar.fillAmount = currentHealth / maxHealth;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public void TakeDamage(float damage)
    {
        if(currentHealth<=0)
            return;
   
        AudioManager.Instance.playSound(AudioManager.Instance.playerHit);
        currentHealth -= damage;
        healthBar.fillAmount = currentHealth / maxHealth;
        if (isPlayerDead())
        {
            player.GetComponent<MovementStateManager>().isDead = true;
        }
          
    }

    private bool isPlayerDead()
    {
        if (currentHealth <= 0)
            return true;

        return false;
    }
}
