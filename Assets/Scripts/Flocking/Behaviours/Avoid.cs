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
        int nThingsToAvoid = 0;
        Vector3 agentPosition = agent.transform.position;
        Vector3 avgPredatorPos = Vector3.zero;
        
        foreach (Transform item in agent.PredatorList)
        {
            Vector3 closestPoint = item.gameObject.GetComponent<Collider>().ClosestPoint(agentPosition);
            
            avgPredatorPos += closestPoint; //Gets average position of predators
            
            nThingsToAvoid++; //Number of predators near
        }

        //Direction opposite to predator position
        avoidanceMove = agentPosition - (-avgPredatorPos/nThingsToAvoid);
        
        return avoidanceMove;
    }
}
