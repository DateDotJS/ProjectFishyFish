using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Obstacle Avoidance")]
public class ObstacleAvoidance : FilteredFlockBehaviour
{
    [SerializeField] private float raycastDistance;
    
    public override Vector3 CalculateFilteredMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        Transform agentTransform = agent.transform;
        Vector3 avoidanceMove = Vector3.zero;
        RaycastHit hit;
        
        // Check if there's an obstacle in front.
        if (Physics.Raycast(agentTransform.position, agentTransform.forward, out hit, 
            raycastDistance, agent.ObstacleLayerMask))
            if (hit.transform != agentTransform)
                avoidanceMove = agentTransform.forward + hit.normal;


        return avoidanceMove;
    }
}
