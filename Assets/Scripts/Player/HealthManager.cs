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

    private List<Material> playerMat = new List<Material>();
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
        foreach (var mat in player.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().materials)
        {
           playerMat.Add(mat);
        }
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
        if (currentHealth < 40)
        {
            foreach (var mat in playerMat)
            {
                mat.EnableKeyword("_EMISSION");
            
                mat.SetColor("_EmissionColor",Color.red *3f);
            }
        }
        if (isPlayerDead())
        {
            foreach (var mat in playerMat)
            {
                mat.EnableKeyword("_EMISSION");
            
                mat.SetColor("_EmissionColor",new Color(0.1f,0.1f,0.1f) *3f);
            }
            player.GetComponent<MovementStateManager>().isDead = true;
        }
          
    }

    public void HealToFull()
    {
        currentHealth = maxHealth;
        foreach (var mat in playerMat)
        {
            mat.EnableKeyword("_EMISSION");
            
            mat.SetColor("_EmissionColor",Color.white *3f);
        }
        healthBar.fillAmount = currentHealth / maxHealth;
    }

    private bool isPlayerDead()
    {
        if (currentHealth <= 0)
            return true;

        return false;
    }
}
