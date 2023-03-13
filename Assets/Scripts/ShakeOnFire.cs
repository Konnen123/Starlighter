using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ShakeOnFire : MonoBehaviour
{
    private CinemachineVirtualCamera _cinemachineVirtualCamera;
    private float startingIntensity;

    private CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;

    private float shakeTimer;
    private float shakeTimerTotal;
    private void Awake()
    {
        _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        
         cinemachineBasicMultiChannelPerlin = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        startingIntensity = cinemachineBasicMultiChannelPerlin.m_FrequencyGain;
    }

    public void ShakeCamera(float intensity, float time)
    {
        cinemachineBasicMultiChannelPerlin.m_FrequencyGain = intensity;
        shakeTimer = time;
        shakeTimerTotal = time;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
            {
                cinemachineBasicMultiChannelPerlin.m_FrequencyGain = Mathf.Lerp(startingIntensity, 0f, 1-(shakeTimer/shakeTimerTotal));
            }
            
        }
    }
}
