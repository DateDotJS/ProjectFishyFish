using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    // stamina / recovery (longterm and shortterm)

    private Animator animator;
    private readonly int swimVelocityParaHash = Animator.StringToHash("SwimVelocity");

    [Header("Movement Physics")]
    [SerializeField] private float drag;
    private Kinematic kinematic;
    private Vector3 externalVelocity;
    private Vector3 momentum;
    private Vector3 angularMomentum; // TODO figure out handling angular momentum

    [Header("Particles")]
    [SerializeField] private ParticleSystem swimmingBubbles;
    [SerializeField] private ParticleSystem eatinggBubbles;

    
    private void Awake()
    {
        kinematic = new Kinematic();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandlePhysics();

        // Calculate transform with kinematic linear movement + any other force applied
        transform.position = transform.position + (kinematic.LinearVel * Time.deltaTime) + (momentum * Time.deltaTime);
        transform.rotation = kinematic.Orientation;

        AnimateSwimming();
    }


    // Handles the water physics of the fish
    private void HandlePhysics()
    {
        // apply any other forces to fish's momentum
        if (externalVelocity != Vector3.zero)
            momentum = externalVelocity;

        // calculate drag factor of the momentum
        momentum = momentum * (1 - Time.deltaTime * drag);
    }

    private void AnimateSwimming()
    {
        // play wobble animation if swimming
        animator.SetFloat(swimVelocityParaHash, kinematic.LinearVel.magnitude);

        // play particles if fish is swimming
        if (kinematic.LinearVel != Vector3.zero && !swimmingBubbles.isPlaying)
            swimmingBubbles.Play();
    }


    #region Public Setters

    // Set kinematic linear velocity
    public void SetVelocity(Vector3 newVelocity) => kinematic.LinearVel = newVelocity;

    // Set kinematic orientation
    public void SetRotation(Quaternion newRotation) => kinematic.Orientation = newRotation;

    // Append an external influence to the player's velocity (i.e. water current).
    public void ApplyExternalForce(Vector3 velocity) => externalVelocity = velocity;

    // Reset the external influence to zero (i.e. leaves water current).
    public void EndExternalForce() => externalVelocity = Vector3.zero;

    #endregion

    public void PlayEatingFX()
    {
        if (!eatinggBubbles.isPlaying)
            eatinggBubbles.Play();
    }


    // FROM UML DIAGRAM ON DIAGRAM.IO
    // ability / unique methods here 
    // use prey ability(s)
    // use predator ability(s)
    // use other ability(s) (repro, fun, etc) 
    // getSurroundings() -> GameObjects
}
