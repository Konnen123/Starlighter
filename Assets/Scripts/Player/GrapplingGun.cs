using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using UnityEngine;
using UnityEngine.Pool;

public class GrapplingGun : MonoBehaviour
{
   [HideInInspector]
   public bool isGrappled;
   
   private LineRenderer lr;
   private Vector3 grapplePoint;
   private Transform cameraTransform;
   private SpringJoint joint;
   private Vector3 currentGrapplePoint;
   [SerializeField] private float maxDistance=100;
   [SerializeField] private LayerMask whatIsGrappleable;
   [SerializeField] private Transform gunTip,player;
   [SerializeField] private float spring, damper, massScale;
  

   private void Awake()
   {
      cameraTransform = Camera.main.gameObject.GetComponent<Transform>();
      lr = GetComponent<LineRenderer>();
   }

   private void Update()
   {
  
      if (Input.GetMouseButtonDown(0))
      {
         StartGrapple();
      }
      else if (Input.GetMouseButtonUp(0))
      {
         StopGrapple();
      }
   }

   private void LateUpdate()
   {
      DrawRope();
   }

   void StartGrapple()
   {
   
      RaycastHit hit;

      if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, maxDistance,whatIsGrappleable))
      {
         isGrappled = true;
         grapplePoint = hit.point;
         joint = player.gameObject.AddComponent<SpringJoint>();
         joint.autoConfigureConnectedAnchor = false;
         joint.connectedAnchor = grapplePoint;

         float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

         joint.maxDistance = distanceFromPoint * .8f;
         joint.minDistance = distanceFromPoint * .25f;

         joint.spring = spring;
         joint.damper = damper;
         joint.massScale = massScale;

         lr.positionCount = 2;
      }
   }

   void DrawRope()
   {
      if(!joint)
         return;
      
      currentGrapplePoint = Vector3.Lerp(currentGrapplePoint,grapplePoint,Time.deltaTime*8f);
      
      lr.SetPosition(0,gunTip.position);
      lr.SetPosition(1,grapplePoint);
   }

   void StopGrapple()
   {
      isGrappled = false;
      lr.positionCount = 0;
      Destroy(joint);
   }
}
