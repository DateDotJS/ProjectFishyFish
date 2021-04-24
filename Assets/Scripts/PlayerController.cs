using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A player controller that controls the player fish by giving 
/// the Fish script a calculated delta position and rotation.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Fish Movement")]
    [SerializeField] private float minForwardSpeed = 3f;
    [SerializeField] private float maxForwardSpeed = 5f;

    [SerializeField] private float maxSideRotation = 0.02f;
    [SerializeField] private float maxPitchRotation = 0.01f;

    [Header("Level Barrier")]
    [Tooltip("Pass in a water current object that will follow and act as a level barrier")]
    [SerializeField] private Transform currentBarrier;
    public float ceilingHeight;

    [Header("Particles")]
    [SerializeField] private ParticleSystem swimmingBubbles;
    [SerializeField] private float swimParticleSpeed;
    [SerializeField] private float swimEmissionRate;
    [SerializeField] private float fastSwimParticleSpeed;
    [SerializeField] private float fastSwimEmissionRate;
    [SerializeField] private ParticleSystem eatingBubbles;

    [Header("Animator")]
    private Animator animator;
    private readonly int swimVelocityParaHash = Animator.StringToHash("SwimVelocity");
    private readonly int isSprintingParaHash = Animator.StringToHash("IsSprinting");

    private Fish fish;

    private void Awake()
    {
        fish = GetComponent<Fish>();
        animator = GetComponent<Animator>();
    }

    void Update() => UpdateMovement();

    private void UpdateMovement()
    {
        var oldPosition = transform.position;

        var forwardMovement = Input.GetButton("Jump");
        var mouseY = Input.GetAxis("Mouse Y");
        var mouseX = Input.GetAxis("Mouse X");

        var deltaMovement = transform.forward * (forwardMovement ? maxForwardSpeed : minForwardSpeed)
            + mouseX * transform.right
            + mouseY * transform.up;

        var newPosition = oldPosition + deltaMovement;

        var newRotation = Quaternion.LookRotation(newPosition - oldPosition);

        fish.SetVelocity(deltaMovement);
        fish.SetRotation(newRotation);

        // play wobble animation if swimming
        animator.SetFloat(swimVelocityParaHash, deltaMovement.magnitude);
        animator.SetBool(isSprintingParaHash, forwardMovement);

        // play particles if fish is swimming
        if (deltaMovement != Vector3.zero)
        {
            var particleMainSettings = swimmingBubbles.main;
            var particleEmissionSettings = swimmingBubbles.emission;

            if (forwardMovement) { // if fast swimming
                particleMainSettings.startSpeed = fastSwimParticleSpeed;
                particleEmissionSettings.rateOverTime = fastSwimEmissionRate;
            }
            else {
                particleMainSettings.startSpeed = swimParticleSpeed;
                particleEmissionSettings.rateOverTime = swimEmissionRate;
            }

            if (!swimmingBubbles.isPlaying) 
                swimmingBubbles.Play();
        }
        else {
            swimmingBubbles.Stop();
         }

        // Let the level barrier follow the player, preventing them from rising a certain level
        // until they evolve/become bigger
        if (currentBarrier != null)
            currentBarrier.position = new Vector3(transform.position.x, ceilingHeight, transform.position.z);
    }

    public void PlayEatingFX()
    {
        if (!eatingBubbles.isPlaying)
            eatingBubbles.Play();
    }
}
