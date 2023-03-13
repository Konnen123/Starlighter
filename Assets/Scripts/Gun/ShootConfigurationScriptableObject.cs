using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShootConfig",menuName = "Guns/Shoot Configuration",order=2)]
public class ShootConfigurationScriptableObject : ScriptableObject
{
    public LayerMask hitMask;
    public Vector3 spread = new Vector3(.1f, .1f, .1f);
    public float fireRate = .25f;

}
