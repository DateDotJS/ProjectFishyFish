using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Steered Cohesion")]
public class SteeredCohesionBehaviour : FilteredFlockBehaviour
{
    private Vector3 currentVelocity;
    [SerializeField] private float agentSmoothTime = 0.5f;

    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        if (filteredContext.Count == 0)
            return Vector3.zero;

        Vector3 cohesionMove = Vector3.zero;
        foreach (Transform item in filteredContext)
        {
            cohesionMove += item.position;
        }

        cohesionMove /= filteredContext.Count;
        cohesionMove -= agent.transform.position;
        cohesionMove = Vector3.SmoothDamp(agent.transform.forward, cohesionMove, ref currentVelocity, agentSmoothTime);

        return cohesionMove;
    }
}

