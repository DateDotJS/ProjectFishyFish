using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Avoid")]
public class Avoid : FilteredFlockBehaviour
{
    public override Vector3 CalculateFilteredMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        //No predator in sight
        if (agent.PredatorList.Count == 0)
        {
            return Vector3.zero;
        }

        Vector3 avoidanceMove = Vector3.zero;
        var nThingsToAvoid = agent.PredatorList.Count;
        Vector3 agentPosition = agent.transform.position;
        Vector3 avgPredatorPos = Vector3.zero;
        
        foreach (Transform item in agent.PredatorList)
        {
            Vector3 closestPoint = item.gameObject.GetComponent<Collider>().ClosestPoint(agentPosition);
            
            avgPredatorPos += closestPoint; // Store total position of predators
        }

        avgPredatorPos /= nThingsToAvoid;

        //Direction opposite to predator position
        avoidanceMove = agentPosition - avgPredatorPos/nThingsToAvoid;
        
        return avoidanceMove;
    }
}
