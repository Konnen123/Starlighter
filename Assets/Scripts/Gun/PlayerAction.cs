using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using MouseButton = UnityEngine.UIElements.MouseButton;

[DisallowMultipleComponent]
public class PlayerAction : MonoBehaviour
{
  [SerializeField] private PlayerGunSelector gunSelector;

  private void Update()
  {
    if (Input.GetMouseButton(0) && gunSelector.activeGun != null)
    {
      gunSelector.activeGun.Shoot();
    }
  }
}
