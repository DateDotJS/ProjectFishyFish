using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FilteredFlockBehaviour : FlockBehaviour
{
    public ContextFilter filter;

    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        List<Transform> filteredContext = (filter is null) ? context : filter.Filter(agent, context);
        return CalculateFilteredMove(agent, filteredContext, flock);
    }

    public abstract Vector3 CalculateFilteredMove(FlockAgent agent, List<Transform> context, Flock flock);
}
