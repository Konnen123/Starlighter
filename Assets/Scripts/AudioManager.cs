using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    public AudioClip laserSound;
    public AudioClip hit,death,playerHit;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>(); 
        Instance = this;
     
    }

    public void playSound(AudioClip clip)
    {
        _audioSource.clip = clip;
        _audioSource.Play();
    }
    
}
