using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    public bool DebugPlayOnStart = false;
    
    [Header("Audio FX")]
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float initialCutoff = 22000f;
    [SerializeField] private float muffledCutoff = 780f;
    [SerializeField] private float tenseMusicSpeed = 1.1f;
    
    [Header("Audio Clips")]
    [SerializeField] private AudioClip ambientClip;
    [SerializeField] private AudioClip mainMusicClip;
    [SerializeField] private AudioClip tenseClip;
    [SerializeField] private AudioClip winClip;
    [SerializeField] private AudioClip loseClip;
    
    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer mainMixer;
    [SerializeField] private AudioMixerGroup ambientGroup;
    [SerializeField] private AudioMixerGroup musicGroup;

    private AudioSource ambientSource;
    private AudioSource musicSource;

    private void Awake()
    {
        // Keep 1 instance of the Audio Manager at all times.
        if (instance != null && instance != this)
            Destroy(gameObject);

        instance = this;
        DontDestroyOnLoad(gameObject);
        
        // Generate the Audio source channels.
        this.ambientSource = gameObject.AddComponent<AudioSource>() as AudioSource;
        this.musicSource = gameObject.AddComponent<AudioSource>() as AudioSource;

        this.ambientSource.volume = 0f;
        this.musicSource.volume = 0f;
        
        this.ambientSource.outputAudioMixerGroup = ambientGroup;
        this.musicSource.outputAudioMixerGroup = musicGroup;

        // Set initial values.
        this.mainMixer.GetFloat("musicCutoff", out initialCutoff);
        this.musicSource.pitch = 1f;
        
        this.ambientSource.clip = this.ambientClip;
        this.ambientSource.loop = true;

        if (this.DebugPlayOnStart)
        {
            PlayTenseMusic();
            PlayAmbientAudio();
        }
    }

    #region Play Audio Clips
    
    public static void PlayAmbientAudio()
    {
        instance.ambientSource.Play();
        FadeAudio(instance.ambientSource, instance.fadeDuration, 1f);
    }
    
    public static void StopAmbientAudio()
    {
        FadeAudio(instance.ambientSource, instance.fadeDuration, 0f);
    }

    public static void PlayMainMusic() => PlayMusic(instance.mainMusicClip);
    
    public static void PlaySpeedupMusic() => instance.musicSource.pitch = instance.tenseMusicSpeed;
    
    public static void StopSpeedupMusic() => instance.musicSource.pitch = 1f;

    public static void PlayTenseMusic() => PlayMusic(instance.tenseClip);

    public static void PlayWinMusic()
    {
        PlayMusic(instance.winClip);
        instance.musicSource.loop = false;
    }
    
    public static void PlayLoseMusic()
    {
        PlayMusic(instance.loseClip);
        instance.musicSource.loop = false;
    }
    
    #endregion

    #region Utility functions

    private static void PlayMusic(AudioClip musicClip)
    {
        if (instance == null)
            return;

        if (instance.musicSource.isPlaying)
            ChangeMusic(musicClip);
        else
        {
            instance.musicSource.clip = musicClip;
            instance.musicSource.loop = true;
            instance.musicSource.Play();
            FadeAudio(instance.musicSource, instance.fadeDuration, 1f);
        }
    }

    public static void PlayMuffledFX() 
        => instance.StartCoroutine(MuffleMusicCo(instance.fadeDuration, instance.muffledCutoff));
    
    public static void StopMuffledFX() 
        => instance.StartCoroutine(MuffleMusicCo(instance.fadeDuration, instance.initialCutoff));

    private static IEnumerator MuffleMusicCo(float duration, float targetCutoff)
    {
        var currentTime = 0f;
        instance.mainMixer.GetFloat("musicCutoff", out var startCutoff);

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            instance.mainMixer.SetFloat("musicCutoff", 
                Mathf.Lerp(startCutoff, targetCutoff, currentTime / duration));
            yield return null;
        }
    }
    
    private static void ChangeMusic(AudioClip newMusic) => instance.StartCoroutine(ChangeMusicCo(newMusic));
    
    private static IEnumerator ChangeMusicCo(AudioClip newMusic)
    {
        FadeAudio(instance.musicSource, instance.fadeDuration, 0f);

        // Wait for fade out to finish.
        while (instance.musicSource.volume > 0)
            yield return new WaitForSeconds(0.01f);
        
        instance.musicSource.Stop();
        instance.musicSource.clip = newMusic;
        instance.musicSource.loop = true;
        instance.musicSource.Play();
        FadeAudio(instance.musicSource, instance.fadeDuration, 1f);
    }
    
    private static void FadeAudio(AudioSource audioSource, float fadeDuration, float targetVolume) 
        => instance.StartCoroutine(FadeAudioCo(audioSource, fadeDuration, targetVolume));
    
    private static IEnumerator FadeAudioCo(AudioSource audioSource, float fadeDuration, float targetVolume)
    {
        var currentTime = 0f;
        var startVolume = audioSource.volume;

        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, currentTime / fadeDuration);
            yield return null;
        }
    }
    
    #endregion
}
