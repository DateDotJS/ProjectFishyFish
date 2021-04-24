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

    private Fish fish;

    private void Awake() => fish = GetComponent<Fish>();

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

        // Let the level barrier follow the player, preventing them from rising a certain level
        // until they evolve/become bigger
        currentBarrier.position = new Vector3(transform.position.x, ceilingHeight, transform.position.z);
    }
}
