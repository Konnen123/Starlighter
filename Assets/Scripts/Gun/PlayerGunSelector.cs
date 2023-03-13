using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerGunSelector : MonoBehaviour
{
    [SerializeField] private GunType gun;
    [SerializeField] private Transform gunParent;
    [SerializeField] private List<GunScriptableObject> guns;


    [Space] [Header("RunTime Filled")] public GunScriptableObject activeGun;

    private void Start()
    {
        GunScriptableObject gun = guns.Find((gun => gun.type == this.gun));

        if (gun == null)
        {
            Debug.LogError($"No GunScriptableObject found for Guntype:{gun}");
            return;
        }

        activeGun = gun;
        gun.Spawn(gunParent,this);

 


    }
}
