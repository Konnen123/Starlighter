using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class MobSpawner : MonoBehaviour
{
    [SerializeField] private GameObject npc;
    [SerializeField] private float mobSpawnerCooldown;
    [SerializeField] private bool isMutant;

    void Start()
    {
        if(!isMutant)
            StartCoroutine(InstantiateMob());
    }


   public IEnumerator InstantiateMob()
    {
        
        GameObject mob = Instantiate(npc, transform);
        mob.transform.localPosition=Vector3.zero;
        yield return new WaitForSeconds(mobSpawnerCooldown);
        StartCoroutine(InstantiateMob());

    }
}
