using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/FastAvoid")]
public class FastAvoid : FilteredFlockBehaviour
{
    private bool speedChanged = false;
    private float extraSpeed = 4.4f;
    
    public override Vector3 CalculateFilteredMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        //No predator in sight
        if (agent.PredatorList.Count == 0)
        {
            //Reset speed to initial one
            if (speedChanged == true)
            {
                speedChanged = false;
            }

            return Vector3.zero;
        }

        Vector3 fastAvoidanceMove = Vector3.zero;
        var nThingsToAvoid = agent.PredatorList.Count;
        Vector3 agentPosition = agent.transform.position;
        Vector3 avgPredatorPos = Vector3.zero;
        
        foreach (Transform item in agent.PredatorList)
        {
            Vector3 closestPoint = item.gameObject.GetComponent<Collider>().ClosestPoint(agentPosition);
            
            avgPredatorPos += closestPoint; // Store total position of predators
        }

        avgPredatorPos /= nThingsToAvoid;
        
        // Increase speed of flock while using fast-avoid behavior
        if (speedChanged == false)
        {
            speedChanged = true;
        }
        
        // Direction opposite to predator position
        fastAvoidanceMove = agentPosition - avgPredatorPos;
        
        return fastAvoidanceMove * (speedChanged ? extraSpeed : 1);
    }
}
