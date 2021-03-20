using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock Behaviour/Composite")]
public class CompositeBehaviour : FlockBehaviour
{
    [SerializeField] private FlockBehaviour[] behaviours;
    [SerializeField] private float[] weights;

    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        if (weights.Length != behaviours.Length)
        {
            Debug.Log("Data mismatch in " + name, this);
            return Vector3.zero;
        }

        Vector3 velocity = Vector3.zero;

        for (int i = 0; i < behaviours.Length; i++)
        {
            Vector3 partialMove = behaviours[i].CalculateMove(agent, context, flock) * weights[i];
            
            if (partialMove != Vector3.zero)
            {
                if (partialMove.sqrMagnitude > weights[i] * weights[i])
                {
                    partialMove.Normalize();
                    partialMove *= weights[i];
                }

                velocity += partialMove;
            }
        }

        return velocity;
    }
}
