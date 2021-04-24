using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Stay In Radius")]
public class StayInRadiusBehaviour : FlockBehaviour
{
    [SerializeField] private Vector3 center;
    [SerializeField] private float radius = 15f;
    [Range(0f, 1f)]
    [SerializeField] private float radiusThreshold = 0.9f;

    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        center = flock.transform.position;
        
        Vector3 centerOffset = this.center - agent.transform.position;

        float relativePositionToCenter = centerOffset.magnitude / this.radius;

        if (relativePositionToCenter < this.radiusThreshold)
        {
            return Vector3.zero;
        }

        //Debug.Log(centerOffset * relativePositionToCenter * relativePositionToCenter);
        return centerOffset * relativePositionToCenter * relativePositionToCenter;
    }
}
