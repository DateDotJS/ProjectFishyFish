using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    [SerializeField] private FlockAgent agentPrefab;
    protected List<FlockAgent> agents = new List<FlockAgent>();
    [SerializeField] private FlockBehaviour behaviour;

    [Range(1, 500)]
    [SerializeField] private int startingCount = 250;
    const float AgentDensity = 0.08f;

    [Range(1f, 100f)]
    [SerializeField] private float driveFactor = 10f;
    [Range(1f, 100f)]
    [SerializeField] protected float maxSpeed = 5f;
    [Range(1f, 10f)]
    [SerializeField] protected float neighbourRadius = 1.5f;
    [Range(10f, 100f)]
    [SerializeField] private float targetRadius = 50f;
    [Range(0f, 1f)]
    [SerializeField] protected float avoidanceRadiusMultiplier = 0.5f;

    protected float squareMaxSpeed;
    protected float squareNeighbourRadius;
    public float SquareAvoidanceRadius { get; set; }

    private FlockManager flockManager;

    private void Awake()
    {
        this.flockManager = FindObjectOfType<FlockManager>();
    }

    void Start()
    {
        this.squareMaxSpeed = this.maxSpeed * this.maxSpeed;
        this.squareNeighbourRadius = this.neighbourRadius * this.neighbourRadius;
        SquareAvoidanceRadius = this.squareNeighbourRadius * this.avoidanceRadiusMultiplier * this.avoidanceRadiusMultiplier;

        for (int i = 0; i < this.startingCount; i++)
        {
            FlockAgent newAgent = Instantiate(
                this.agentPrefab,
                Random.insideUnitSphere * this.startingCount * AgentDensity + transform.position,
                Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)),
                transform
            );

            newAgent.name = ($"Agent {i}");
            newAgent.AgentFlock = this;
            this.agents.Add(newAgent);
        }

        this.flockManager.AddFlock(this);
    }

    void Update()
    {
        foreach (FlockAgent agent in this.agents)
        {
            agent.GetNearbyObjects(neighbourRadius);
            agent.GetNearbyPredators(targetRadius);

            Vector3 velocity = this.behaviour.CalculateMove(agent, agent.Context, this, Time.deltaTime);
            velocity *= this.driveFactor;
            if (velocity.sqrMagnitude > this.squareMaxSpeed)
            {
                velocity = velocity.normalized * this.maxSpeed;
            }
            
            agent.Kinematics.LinearVel = velocity;
            agent.Move();
        }
    }

    public void ChangeFlockSpeed(float newSpeed)
    {
        maxSpeed = newSpeed;
    }

    public float GetFlockSpeed()
    {
        return maxSpeed;
    }

    public void SetBehaviour(FlockBehaviour behaviour)
    {
        this.behaviour = behaviour;
    }

    public void RemoveAgent(FlockAgent agent)
    {
        agents.Remove(agent);

        if(agents.Count == 0) {
            Destroy(gameObject);
        }
    }

    public List<Collider> GetAgentsWithinRange(FlockAgent self, Vector3 position, float radius)
    {
        List<Collider> neighbourAgents = new List<Collider>();
        foreach(FlockAgent agent in agents) {
            if(agent != self) {
                if(Vector3.Distance(position, agent.transform.position) <= radius) {
                    neighbourAgents.Add(agent.gameObject.GetComponent<Collider>());
                }
            }
        }
        return neighbourAgents;
    }
}
