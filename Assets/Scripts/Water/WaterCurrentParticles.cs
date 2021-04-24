using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCurrentParticles : MonoBehaviour
{
    [Tooltip("Emission rate of the particle system.")]
    [SerializeField] private float emissionRate = 50f;
    [SerializeField] private float speedFactor = 1f;

    private ParticleSystem particleSystem;
    private WaterCurrent waterCurrent;

    private void Awake()
    {
        waterCurrent = GetComponent<WaterCurrent>();
        particleSystem = GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        InitializeParticleSystem();
    }

    private void InitializeParticleSystem()
    {
        particleSystem.Stop();

        var speed = waterCurrent.Velocity.magnitude * speedFactor;
        var particleMainSettings = this.particleSystem.main;
        var particleEmissionSettings = this.particleSystem.emission;
        
        particleMainSettings.startSpeed = speed;
        particleMainSettings.startLifetime = transform.localScale.z / speed;
        particleEmissionSettings.rateOverTime = emissionRate * speed;
        
        particleSystem.Play();
    }
}
