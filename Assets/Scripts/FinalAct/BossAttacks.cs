using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BossAttacks : MonoBehaviour
{
   [SerializeField] private List<GameObject> lasers;
   [SerializeField] private List<GameObject> eyes,zombieSpawners;
   [SerializeField] private GameObject mutantSpawner;
   [SerializeField] private List<EyeShooting> shootingEyes;


   
   private void Start()
   {

      StartCoroutine(firstEncounter());
      foreach (var laser in lasers)
      {
         laser.SetActive(false);
      }
      foreach (var eye in eyes)
      {
         eye.SetActive(false);
      }
      mutantSpawner.SetActive(false);
   }

   private void Update()
   {
      if(BossHealthManager.Instance.isHalfHp)
      {
         foreach (var laser in lasers)
         {
            laser.SetActive(true);
         }

         foreach (var eye in eyes)
         {
            eye.SetActive(true);
         }
         foreach (var shootingEye in shootingEyes)
         {
            shootingEye.shootRate = 2;
         }
         foreach (var zombieSpawner in zombieSpawners)
         {
            zombieSpawner.SetActive(false);
         }

         if (!mutantSpawner.activeInHierarchy)
         {
            mutantSpawner.SetActive(true);
            StartCoroutine(halfHpText());
            StartCoroutine(mutantSpawner.GetComponent<MobSpawner>().InstantiateMob());
         }
      }
      
      if(BossHealthManager.Instance.isLowHp)
      {
        
         foreach (var shootingEye in shootingEyes)
         {
            shootingEye.shootRate = 1;
         }
         
      }
   }
   IEnumerator firstEncounter()
   {
      TipText.Instance.SetText("You would fight me?! Come, let me show you hell!");
      yield return new WaitForSeconds(5f);
      TipText.Instance.SetText("");
   }

   IEnumerator halfHpText()
   {
      TipText.Instance.SetText("I WILL NOT YIELD!");
      yield return new WaitForSeconds(5f);
      TipText.Instance.SetText("");
   }

   public void BackToMenu()
   {
      SceneManager.LoadScene(0);
   }
   
}
