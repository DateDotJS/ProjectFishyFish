using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlayerController : MonoBehaviour
{
    [SerializeField] private float minForwardSpeed = 3f;
    [SerializeField] private float maxForwardSpeed = 5f;

    [SerializeField] private float maxSideRotation = 0.02f;
    [SerializeField] private float maxPitchRotation = 0.01f;

    private Vector3 externalVelocity = Vector3.zero;
    private Rigidbody rigidBody;

    private void Awake() => rigidBody = GetComponent<Rigidbody>();

    void Update() => UpdateMovement();

    private void UpdateMovement()
    {
        HandlePhysics();

        var oldPosition = transform.position;

        var forwardMovement = Input.GetButton("Jump");
        var mouseY = Input.GetAxis("Mouse Y");
        var mouseX = Input.GetAxis("Mouse X");

        var deltaMovement = transform.forward * (forwardMovement ? maxForwardSpeed : minForwardSpeed)
            + mouseX * transform.right
            + mouseY * transform.up;

        var newPosition = oldPosition + deltaMovement * Time.deltaTime;

        var newRotation = Quaternion.LookRotation(newPosition - oldPosition);

        transform.SetPositionAndRotation(newPosition, newRotation);
    }


    // We handle water physics for our player fish here.
    private void HandlePhysics()
    {
        // If external factor is zero, we let RigidBody's linear drag dwindle the velocity to zero.
        if (externalVelocity != Vector3.zero)
            rigidBody.velocity = externalVelocity;
    }
    
    // Append an external influence to the player's velocity (i.e. water current).
    public void AppendForce(Vector3 velocity) => externalVelocity = velocity;

    // Reset the external influence to zero (i.e. leaves water current).
    public void ResetForce() => externalVelocity = Vector3.zero;
}
