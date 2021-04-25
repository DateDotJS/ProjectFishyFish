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
    [SerializeField] public float MinForwardSpeed = 3f;
    [SerializeField] public float MaxForwardSpeed = 5f;

    [SerializeField] private float maxSideRotation = 0.02f;
    [SerializeField] private float maxPitchRotation = 0.01f;

    [SerializeField] private float lookAngleBounds = 85f;

    [Header("Level Barrier")]
    [Tooltip("Pass in a water current object that will follow the player and act as a level barrier")]
    [SerializeField] private Transform ceilingCurrentPrefab;
    public float ceilingHeight;

    [Header("Particles")]
    [SerializeField] private ParticleSystem swimmingBubbles;
    [SerializeField] private float swimParticleSpeed;
    [SerializeField] private float swimEmissionRate;
    [SerializeField] private float fastSwimParticleSpeed;
    [SerializeField] private float fastSwimEmissionRate;

    [Header("Animator")]
    private Animator animator;
    private readonly int swimVelocityParaHash = Animator.StringToHash("SwimVelocity");
    private readonly int isSprintingParaHash = Animator.StringToHash("IsSprinting");

    private Fish fish;
    private ParticleSystem.MainModule particleMainSettings;
    private ParticleSystem.EmissionModule particleEmissionSettings;

    private void Awake()
    {
        fish = GetComponent<Fish>();
        animator = GetComponent<Animator>();

        particleMainSettings = swimmingBubbles.main;
        particleEmissionSettings = swimmingBubbles.emission;

        ceilingCurrentPrefab = Instantiate(ceilingCurrentPrefab, Vector3.zero, ceilingCurrentPrefab.rotation);
    }

    void Update() => UpdateMovement();

    private void UpdateMovement()
    {
        var oldPosition = transform.position;
        var oldRotation = transform.rotation;

        var forwardMovement = Input.GetButton("Jump");
        var mouseY = Input.GetAxis("Mouse Y");
        var mouseX = Input.GetAxis("Mouse X");

        var deltaMovement = transform.forward * (forwardMovement ? MaxForwardSpeed : MinForwardSpeed)
            + mouseX * transform.right
            + mouseY * transform.up;

        var newPosition = oldPosition + deltaMovement;

        var newRotation = Quaternion.LookRotation(newPosition - oldPosition);

        if (IsTooPitched(newRotation))
            newRotation = oldRotation;

        fish.SetVelocity(deltaMovement);
        fish.SetRotation(newRotation);

        // play wobble animation if swimming
        animator.SetFloat(swimVelocityParaHash, deltaMovement.magnitude);
        animator.SetBool(isSprintingParaHash, forwardMovement);

        // play particles if fish is swimming
        if (swimmingBubbles != null && deltaMovement != Vector3.zero)
        {
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

        // Let the level barrier follow the player, preventing them from rising a certain level
        // until they evolve/become bigger
        if (ceilingCurrentPrefab != null)
            ceilingCurrentPrefab.position = new Vector3(transform.position.x, ceilingHeight, transform.position.z);
    }

    private bool IsTooPitched(Quaternion newRotation)
    {
        return Vector3.Angle(Vector3.up, newRotation * Vector3.forward) < (90 - this.lookAngleBounds)
                    || Vector3.Angle(Vector3.down, newRotation * Vector3.forward) < (90 - this.lookAngleBounds);
    }
}
