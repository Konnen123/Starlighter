using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Pool;
using UnityEngine.UI;

public class ThirdPersonShooterController : MonoBehaviour
{
    [SerializeField]private TrailConfigScriptableObject trailConfig;

     public GameObject rifle;
    [SerializeField] private ParticleSystem onEnemyHitParticle;
    private ParticleSystem particle;
    
    [SerializeField]
    private CinemachineVirtualCamera aimVirtualCamera;

    [SerializeField] private float cameraShakeIntensity = 5f;
    [SerializeField] private float cameraShakeDuration = 1f;
    

    public GameObject crossHair;

    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
  

    [SerializeField] private float fireRate = 1f;
    [SerializeField] private Transform bulletTransform;

    [SerializeField] private float rifleDamgage=20;
    [SerializeField] private float shootDistance = 200;
    [SerializeField] private Color enemyFoundCrosshair;
    
    private MovementStateManager movementStateManager;
    private ThirdPersonCamera thirdPersonCamera;
    private RigBuilder rigBuilder;
    private ObjectPool<TrailRenderer> trailPool;
    private Camera camera;
    private Vector2 screenCenterPoint;
    private float currentShootCooldown;

    private Color defaultCrosshairColor;
    private Image crosshairImage;

    


    private void Awake()
    {
        camera = Camera.main;
        particle = rifle.GetComponentInChildren<ParticleSystem>();
        rigBuilder = GetComponent<RigBuilder>();
        thirdPersonCamera = camera.GetComponent<ThirdPersonCamera>();
        movementStateManager = GetComponent<MovementStateManager>();
        trailPool = new ObjectPool<TrailRenderer>(CreateTrail);

        crosshairImage = crossHair.GetComponentInChildren<Image>();
        
        defaultCrosshairColor = crosshairImage.color;
        
        rifle.SetActive(false);

    }

   

    private void Update()
    {
        Vector3 mouseWorldPosition = Vector3.zero;
        
        screenCenterPoint = new Vector2(Screen.width/2f,Screen.height/2f);
        Ray ray = camera.ScreenPointToRay(screenCenterPoint);

        Transform hitTransform = null;
        
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            mouseWorldPosition = raycastHit.point;
            hitTransform = raycastHit.transform;
        }
        
        if (Input.GetMouseButton(1))
        {
            rifle.SetActive(true);
            rigBuilder.enabled = true;
        
            rifle.transform.LookAt(raycastHit.point);
            crossHair.SetActive(true);
            aimVirtualCamera.gameObject.SetActive(true);
            movementStateManager.SetSensitivity(aimSensitivity);
            thirdPersonCamera.SetRotateOnMove(false);

            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
            
            crosshairImage.color = defaultCrosshairColor;
            
            //crosshair will be red if enemy is detected
            if(isShootable(raycastHit.collider.gameObject.layer) && Vector3.Distance(transform.position, hitTransform.position) <= shootDistance)
                crosshairImage.color = enemyFoundCrosshair;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
            

            if (Input.GetMouseButton(0) &&currentShootCooldown>=fireRate)
            {
                particle.Play();
                AudioManager.Instance.playSound(AudioManager.Instance.laserSound);
                currentShootCooldown = 0;
                aimVirtualCamera.GetComponent<ShakeOnFire>().ShakeCamera(cameraShakeIntensity,cameraShakeDuration);
                StartCoroutine(PlayTrail(bulletTransform.position, raycastHit.point, raycastHit));
                
                if (hitTransform != null)
                {
                    if (Vector3.Distance(transform.position, hitTransform.position) > shootDistance)
                        return;
                    
                
                    if (raycastHit.collider.gameObject.layer == 8)
                    { 
                        ParticleSystem enemyHitParticle =  Instantiate(onEnemyHitParticle, raycastHit.point, Quaternion.identity);
                        Destroy(enemyHitParticle.gameObject,1f);
                        StartCoroutine(hitAudio());

                        EnemyAi enemyAi = raycastHit.collider.GetComponent<EnemyAi>();
                        if(enemyAi==null)
                            return;
                        enemyAi.TakeDamage(rifleDamgage);
                    }

                    if (raycastHit.collider.gameObject.layer == 11)
                    {
                        Destroy(raycastHit.collider.gameObject);
                    }
                    if (raycastHit.collider.gameObject.layer == 12)
                    {
                        StartCoroutine(hitAudio());
                        raycastHit.collider.GetComponent<ShootingCamera>().TakeDamage(rifleDamgage);
                    }
                    if (raycastHit.collider.gameObject.layer == 13)
                    {
                        BossHealthManager.Instance.TakeDamage(rifleDamgage);
                    }
                    if (raycastHit.collider.gameObject.layer == 14)//boss weakSpot
                    {
                        BossHealthManager.Instance.TakeDamage(rifleDamgage*2);
                        Debug.Log("weakspot");
                    }

                    if (raycastHit.collider.gameObject.layer == 15)
                    {
                        raycastHit.collider.GetComponent<FinalActNpc>().enableRagdoll();
                    }

   
                }
               
             
            }
                
            
        }
        else
        {
            rigBuilder.enabled = false;
            crossHair.SetActive(false);
            aimVirtualCamera.gameObject.SetActive(false);
            movementStateManager.SetSensitivity(normalSensitivity);
            thirdPersonCamera.SetRotateOnMove(true);
            rifle.SetActive(false);
    
        }

        if (currentShootCooldown < fireRate)
        {
            currentShootCooldown += Time.deltaTime;
        }

    }
    
    private IEnumerator PlayTrail(Vector3 startPoint, Vector3 endPoint, RaycastHit hit)
    {
        TrailRenderer instance = trailPool.Get();
        instance.gameObject.SetActive(true);
        instance.transform.position = startPoint;
        yield return null;

        instance.emitting = true;

        float distance = Vector3.Distance(startPoint, endPoint);
        float remainingDistance = distance;
        while (remainingDistance > 0)
        {
            instance.transform.position =
                Vector3.Lerp(startPoint, endPoint, Mathf.Clamp01(1 - (remainingDistance / distance)));
            remainingDistance -= trailConfig.simulationSpeed * Time.deltaTime;

            yield return null;
        }

        instance.transform.position = endPoint;

      
        
        yield return new WaitForSeconds(trailConfig.duration);
        yield return null;
        instance.emitting = false;
        instance.gameObject.SetActive(false);
        
        trailPool.Release(instance);
    }

    IEnumerator hitAudio()
    {
        yield return new WaitForSeconds(.3f);
        AudioManager.Instance.playSound(AudioManager.Instance.hit);
    }
    private TrailRenderer CreateTrail()
    {
        GameObject instance = new GameObject("Bullet Trail");
        TrailRenderer trail = instance.AddComponent<TrailRenderer>();
        trail.colorGradient = trailConfig.color;
        trail.material = trailConfig.material;
        trail.widthCurve = trailConfig.widthCurve;
        trail.time = trailConfig.duration;
        trail.minVertexDistance = trailConfig.minVertexDistance;
        
        trail.emitting = false;
        trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        return trail;
    }

    bool isShootable(int layer)
    {
        if (layer == 8 || (layer >= 11 && layer <= 15))
            return true;
        return false;
    }
}
