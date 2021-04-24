using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FlockAgent))]
public class LoneFlock : Flock
{
    private GameObject target;

    void Start()
    {
        this.squareMaxSpeed = this.maxSpeed * this.maxSpeed;
        this.squareNeighbourRadius = this.neighbourRadius * this.neighbourRadius;
        SquareAvoidanceRadius = this.squareNeighbourRadius * this.avoidanceRadiusMultiplier * this.avoidanceRadiusMultiplier;

        FlockAgent agent = GetComponent<FlockAgent>();
        agent.name = ("Self");
        agent.AgentFlock = this;
        this.agents.Add(agent);
    }

    public void SetTarget(GameObject o)
    {
        target = o;
    }

    public GameObject GetTarget()
    {
        return target;
    }
}
