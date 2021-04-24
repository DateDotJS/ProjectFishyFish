using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    // stamina / recovery (longterm and shortterm)

    [Header("Movement Physics")]
    [SerializeField] private float drag;
    private Kinematic kinematic;
    private Vector3 externalVelocity;
    private Vector3 momentum;
    private Vector3 angularMomentum; // TODO figure out handling angular momentum

    
    private void Awake() => kinematic = new Kinematic();

    void Update()
    {
        HandlePhysics();

        // Calculate transform with kinematic linear movement + any other force applied
        transform.position = transform.position + (kinematic.LinearVel * Time.deltaTime) + (momentum * Time.deltaTime);
        transform.rotation = kinematic.Orientation;
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


    // FROM UML DIAGRAM ON DIAGRAM.IO
    // ability / unique methods here 
    // use prey ability(s)
    // use predator ability(s)
    // use other ability(s) (repro, fun, etc) 
    // getSurroundings() -> GameObjects
}
