using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

public class FishAudio : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] swimClips;
    [SerializeField] private AudioClip[] dashClips;
    [SerializeField] private AudioClip[] eatClips;
    
    [Header("Mixer Groups")]
    [SerializeField] private AudioMixerGroup sfxGroup;
    [SerializeField] private AudioSource audioSource;

    private void Awake() => this.audioSource.outputAudioMixerGroup = this.sfxGroup;

    public void PlaySwimAudio()
    {
        if (this.audioSource.isPlaying)
            return;

        var index = Random.Range(0, this.swimClips.Length);

        this.audioSource.clip = this.swimClips[index];
        this.audioSource.Play();
    }
    
    public void PlayDashAudio()
    {
        var index = Random.Range(0, this.dashClips.Length);

        this.audioSource.PlayOneShot(this.dashClips[index]);
    }
    
    public void PlayEatAudio()
    {
        var index = Random.Range(0, this.eatClips.Length);

        this.audioSource.PlayOneShot(this.eatClips[index]);
    }
}
