using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Composite")]
public class CompositeBehaviour : FlockBehaviour
{
    public FlockBehaviour[] Behaviours;
    public float[] Weights;

    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock, float time)
    {
        if (Weights.Length != Behaviours.Length)
        {
            Debug.Log($"Data mismatch in {this}");
            return Vector3.zero;
        }

        Vector3 velocity = Vector3.zero;

        for (int i = 0; i < Behaviours.Length; i++)
        {
            Vector3 partialMove = Behaviours[i].CalculateMove(agent, context, flock, time) * Weights[i];
            
            if (partialMove != Vector3.zero)
            {
                if (partialMove.sqrMagnitude > Weights[i] * Weights[i])
                {
                    partialMove.Normalize();
                    partialMove *= Weights[i];
                }

                velocity += partialMove;
            }
        }

        return velocity;
    }
}
