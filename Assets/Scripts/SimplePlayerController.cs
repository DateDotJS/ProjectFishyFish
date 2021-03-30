using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlayerController : MonoBehaviour
{
    [SerializeField] private float minForwardSpeed = 3f;
    [SerializeField] private float maxForwardSpeed = 5f;

    [SerializeField] private float maxSideRotation = 0.02f;
    [SerializeField] private float maxPitchRotation = 0.01f;

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

        var newPosition = oldPosition + deltaMovement * Time.deltaTime;

        var newRotation = Quaternion.LookRotation(newPosition - oldPosition);

        transform.SetPositionAndRotation(newPosition, newRotation);
    }
}
