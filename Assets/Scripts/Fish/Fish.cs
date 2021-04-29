using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FishType
{
    SmallFish,
    BigFish,
    RedFish,
    BlueFish,
    GreenFish, 
    PurpleFish,
    OrangeFish,
}

public class Fish : MonoBehaviour
{
    public FishType FishType;
    public Food food;

    [Header("Movement Physics")]
    [SerializeField] private float drag;
    private Kinematic kinematic;
    private Vector3 externalVelocity;
    private Vector3 momentum;
    private Vector3 angularMomentum; // TODO figure out handling angular momentum
    private Vector3 lastPos;
    private bool collidedToWall = false;
    
    private void Awake() {
        this.kinematic = new Kinematic();
        this.food = GetComponent<Food>();
    }

    void Update()
    {
        lastPos = transform.position;

        // Calculate transform with kinematic linear movement + any other force applied
        transform.position = transform.position + (kinematic.LinearVel * Time.deltaTime) + (momentum * Time.deltaTime);
        transform.rotation = kinematic.Orientation;

        HandlePhysics();
    }


    // Handles the water physics of the fish
    private void HandlePhysics()
    {
        // apply any other forces to fish's momentum
        if (externalVelocity != Vector3.zero)
            momentum = externalVelocity;

        // calculate drag factor of the momentum
        momentum = momentum * (1 - Time.deltaTime * drag);

        // if collided on a wall, reset to the last position before colliding
        if (collidedToWall)
            transform.position = lastPos;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
            collidedToWall = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
            collidedToWall = false;
    }
}