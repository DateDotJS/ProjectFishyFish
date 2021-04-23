using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A player controller that controls the player fish by giving 
/// the Fish script a calculated delta position and rotation.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float minForwardSpeed = 3f;
    [SerializeField] private float maxForwardSpeed = 5f;

    [SerializeField] private float maxSideRotation = 0.02f;
    [SerializeField] private float maxPitchRotation = 0.01f;

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
    }
}
