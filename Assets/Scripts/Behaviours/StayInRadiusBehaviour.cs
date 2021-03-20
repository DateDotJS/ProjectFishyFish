using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock Behaviour/Stay In Radius")]
public class StayInRadiusBehaviour : FlockBehaviour
{
    [SerializeField] private Vector3 center;
    [SerializeField] private float radius = 15f;
    [Range(0f, 1f)]
    [SerializeField] private float radiusThreshold = 0.9f;

    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        Vector3 centerOffset = center - agent.transform.position;
        float relativePositionToCenter = centerOffset.magnitude / radius;

        if (relativePositionToCenter < radiusThreshold)
        {
            return Vector3.zero;
        }

        return centerOffset * relativePositionToCenter * relativePositionToCenter;
    }
}
