using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    [SerializeField] private FlockAgent agentPrefab;
    List<FlockAgent> agents = new List<FlockAgent>();
    [SerializeField] private FlockBehaviour behaviour;

    [Range(1, 500)]
    [SerializeField] private int startingCount = 250;
    const float AgentDensity = 0.08f;

    [Range(1f, 100f)]
    [SerializeField] private float driveFactor = 10f;
    [Range(1f, 100f)]
    [SerializeField] private float maxSpeed = 5f;
    [Range(1f, 10f)]
    [SerializeField] private float neighbourRadius = 1.5f;
    [Range(0f, 1f)]
    [SerializeField] private float avoidanceRadiusMultiplier = 0.5f;

    private float squareMaxSpeed;
    private float squareNeighbourRadius;
    public float SquareAvoidanceRadius { get; set; }

    void Start()
    {
        this.squareMaxSpeed = this.maxSpeed * this.maxSpeed;
        this.squareNeighbourRadius = this.neighbourRadius * this.neighbourRadius;
        SquareAvoidanceRadius = this.squareNeighbourRadius * this.avoidanceRadiusMultiplier * this.avoidanceRadiusMultiplier;

        for (int i = 0; i < this.startingCount; i++)
        {
            FlockAgent newAgent = Instantiate(
                this.agentPrefab,
                Random.insideUnitSphere * this.startingCount * AgentDensity,
                Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)),
                transform
            );

            newAgent.name = ($"Agent {i}");
            newAgent.AgentFlock = this;
            this.agents.Add(newAgent);
        }
    }

    void Update()
    {
        foreach (FlockAgent agent in this.agents)
        {
            agent.GetNearbyObjects(neighbourRadius);

            Vector3 velocity = this.behaviour.CalculateMove(agent, agent.Context, this);
            velocity *= this.driveFactor;
            if (velocity.sqrMagnitude > this.squareMaxSpeed)
            {
                velocity = velocity.normalized * this.maxSpeed;
            }
            
            agent.Kinematics.LinearVel = velocity;
            agent.Move();
        }
    }
}
