using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FlockAgent))]
public class LoneFlock : Flock
{
    [SerializeField] private float wanderDelay;

    private GameObject target;

    private float wanderTimer;
    private Vector3 wanderVariation;
    private Vector3 wanderStart;

    private void Awake()
    {
        wanderTimer = wanderDelay;
        wanderVariation = new Vector3();
    }

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

    public bool SetWanderTimer(float time)
    {
        wanderTimer += time;

        if(wanderTimer >= wanderDelay) {
            wanderTimer = 0;
            return true;
        } else {
            return false;
        }
    }

    public float GetWanderTimer()
    {
        return wanderTimer;
    }

    public float GetWanderDelay()
    {
        return wanderDelay;
    }

    public void SetWanderVariation(Vector3 variation)
    {
        wanderVariation = variation;
    }

    public Vector3 GetWanderVariation()
    {
        return wanderVariation;
    }

    public void SetWanderStart(Vector3 position)
    {
        wanderStart = position;
    }

    public Vector3 GetWanderStart()
    {
        return wanderStart;
    }
}
