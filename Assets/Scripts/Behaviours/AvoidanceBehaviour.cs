using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Avoidance")]
public class AvoidanceBehaviour : FilteredFlockBehaviour
{
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        List<Transform> filteredContext = (filter is null) ? context : filter.Filter(agent, context);
        if (filteredContext.Count == 0)
            return Vector3.zero;

        Vector3 avoidanceMove = Vector3.zero;
        int nThingsToAvoid = 0;
        foreach (Transform item in filteredContext)
        {
            Vector3 closestPoint = item.gameObject.GetComponent<Collider>().ClosestPoint(agent.transform.position);
            if (Vector3.SqrMagnitude(closestPoint - agent.transform.position) < flock.SquareAvoidanceRadius)
            {
                nThingsToAvoid++;
                avoidanceMove += agent.transform.position - closestPoint;
            }
        }

        if (nThingsToAvoid > 0)
            avoidanceMove /= nThingsToAvoid;

        return avoidanceMove;
    }
}

