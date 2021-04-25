using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Split Behaviour")]
public class SplitBehaviour : FilteredFlockBehaviour
{
    private bool speedChanged = false;
    public float ExtraSpeed = 2.4f;
    public float SplitDistance = 5f;
    
    public override Vector3 CalculateFilteredMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        //No predator in sight
        if (agent.PredatorList.Count == 0)
        {
            //Reset speed to initial one
            if (speedChanged == true)
            {
                //flock.ChangeFlockSpeed(flock.GetFlockSpeed() / ExtraSpeed);
                speedChanged = false;
            }

            return Vector3.zero;
        }

        var splitDirection = Vector3.zero;
        var nThingsToAvoid = agent.PredatorList.Count;
        var agentPosition = agent.transform.position;
        var avgPredatorPos = Vector3.zero;
        var avgPredatorRight = Vector3.zero;
        foreach (Transform predator in agent.PredatorList)
        {
            Vector3 closestPoint = predator.gameObject.GetComponent<Collider>().ClosestPoint(agentPosition);
            
            avgPredatorPos += closestPoint; //Gets average position of predators
            avgPredatorRight += predator.transform.right;
        }
        avgPredatorPos /= nThingsToAvoid;
        avgPredatorRight /= nThingsToAvoid;
        
        if (speedChanged == false)
        {
            //flock.ChangeFlockSpeed(flock.GetFlockSpeed() * ExtraSpeed);
            speedChanged = true;
        }
        
        // Split direction.
        Vector3 distance = agentPosition - avgPredatorPos;
        var side = Vector3.Dot(distance, avgPredatorRight);
        splitDirection = avgPredatorRight * ((side < 0 ? -1 : 1) * SplitDistance);
        
        return splitDirection * (speedChanged ? ExtraSpeed : 1);
    }
}
