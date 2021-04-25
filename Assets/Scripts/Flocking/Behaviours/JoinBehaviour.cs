using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Join Behaviour")]
public class JoinBehaviour : FilteredFlockBehaviour
{
    private bool isJoining = false;
    public float JoinDistance = 5f;

    private Vector3 lastAvgPredatorPos = Vector3.zero;
    private Vector3 avgPredatorRight = Vector3.zero;
    
    public override Vector3 CalculateFilteredMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        if (agent.PredatorList.Count != 0)
        {
            lastAvgPredatorPos = Vector3.zero;
            
            var nThingsToAvoid = agent.PredatorList.Count;
            foreach (Transform predator in agent.PredatorList)
            {
                this.lastAvgPredatorPos += predator.position;
                avgPredatorRight += predator.transform.right;
            }

            this.lastAvgPredatorPos /= nThingsToAvoid;
            avgPredatorRight /= nThingsToAvoid;

            return Vector3.zero;
        }

        // Join direction.
        Vector3 distance = agent.transform.position - lastAvgPredatorPos;
        var side = Vector3.Dot(distance, avgPredatorRight);
        var joinDirection = -avgPredatorRight * ((side < 0 ? -1 : 1) * JoinDistance);
        return joinDirection;
    }
}
