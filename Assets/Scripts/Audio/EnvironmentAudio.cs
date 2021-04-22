using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Audio for any other objects located in 3D space that are not fishes.
///
/// <example>Water currents, geysers, bubbles, etc.</example>
/// </summary>
public class EnvironmentAudio : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip waterCurrentClip;
    
    [Header("Mixer Groups")]
    [SerializeField] private AudioMixerGroup sfxGroup;
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        this.audioSource.outputAudioMixerGroup = this.sfxGroup;
        
        PlayWaterCurrentAudio();
    }

    public void PlayWaterCurrentAudio()
    {
        this.audioSource.clip = waterCurrentClip;
        this.audioSource.loop = true;
        this.audioSource.Play();
    }

    public void StopAudio() => this.audioSource.Stop();
}
