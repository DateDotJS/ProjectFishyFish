using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    [SerializeField] private FlockAgent agentPrefab;
    List<FlockAgent> agents = new List<FlockAgent>();
    [SerializeField] private FlockBehaviour behaviour;

    [Range(10, 500)]
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
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighbourRadius = neighbourRadius * neighbourRadius;
        SquareAvoidanceRadius = squareNeighbourRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

        for (int i = 0; i < startingCount; i++)
        {
            FlockAgent newAgent = Instantiate(
                agentPrefab,
                Random.insideUnitSphere * startingCount * AgentDensity,
                Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)),
                transform
            );

            newAgent.name = "Agent " + i;
            agents.Add(newAgent);
        }
    }

    void Update()
    {
        foreach (FlockAgent agent in agents)
        {
            List<Transform> context = GetNearbyObjects(agent);

            Vector3 velocity = behaviour.CalculateMove(agent, context, this);
            velocity *= driveFactor;
            if(velocity.sqrMagnitude > squareMaxSpeed)
            {
                velocity = velocity.normalized * maxSpeed;
            }
            agent.Move(velocity);
        }
    }

    List<Transform> GetNearbyObjects(FlockAgent agent)
    {
        List<Transform> context = new List<Transform>();
        Collider[] contextColliders = Physics.OverlapSphere(agent.transform.position, neighbourRadius);
        foreach (Collider collider in contextColliders)
        {
            if(collider != agent.AgentCollider)
            {
                context.Add(collider.transform);
            }
        }

        return context;
    }
}
