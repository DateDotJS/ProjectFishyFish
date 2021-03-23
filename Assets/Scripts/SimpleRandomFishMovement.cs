using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRandomFishMovement : MonoBehaviour
{
    [SerializeField] private float minForwardSpeed = 3f;
    [SerializeField] private float maxForwardSpeed = 5f;

    [SerializeField] private float maxSideRotation = 0.02f;
    [SerializeField] private float maxPitchRotation = 0.01f;

    void Update() => UpdateMovement();

    private void UpdateMovement()
    {
        var oldPosition = transform.position;

        var newPosition = oldPosition
            + transform.forward * Random.Range(this.minForwardSpeed, this.maxForwardSpeed) * Time.deltaTime
            + transform.right * Random.Range(-this.maxSideRotation, this.maxSideRotation) * Time.deltaTime
            + transform.up * Random.Range(-this.maxPitchRotation, this.maxPitchRotation) * Time.deltaTime;
        var newRotation = Quaternion.LookRotation(newPosition - oldPosition);

        transform.SetPositionAndRotation(newPosition, newRotation);
    }
}
