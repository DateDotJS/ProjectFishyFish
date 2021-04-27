using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlockState
{
    Chillin,
    PredatorPresence,
    PredatorChase,
    PredatorAttack
}

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

    private Animator animator;
    private FlockState flockState = FlockState.Chillin;

    private GameObject predator;
    [SerializeField] private float minPredatorPresenceDistance;
    [SerializeField] private float minPredatorChaseDistance;
    [SerializeField] private float minPredatorAttackDistance;

    protected FlockManager flockManager;

    private void Awake()
    {
        this.flockManager = FindObjectOfType<FlockManager>();
    }

    void Start()
    {
        animator = GetComponent<Animator>();

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
        float flockPredatorDistance = float.MaxValue;

        if (predator != null)
            flockPredatorDistance = Vector3.Distance(this.transform.position, predator.transform.position);

        if(!this.transform.CompareTag("Predator"))
            switch (this.flockState)
            {
                case FlockState.PredatorPresence:
                    if (flockPredatorDistance <= this.minPredatorChaseDistance)
                    {
                        AlertFlockOfPredatorChase();
                    }
                    else if (flockPredatorDistance > this.minPredatorPresenceDistance)
                    {
                        GoBackToChillin();
                    }
                    break;
                case FlockState.PredatorChase:
                    if (flockPredatorDistance <= this.minPredatorAttackDistance)
                    {
                        AlertFlockOfPredatorAttack();
                    }
                    else if (flockPredatorDistance > this.minPredatorChaseDistance)
                    {
                        AlertFlockOfPredatorPresence();
                    }
                    break;
                case FlockState.PredatorAttack:
                    break;
            }

        foreach (FlockAgent agent in this.agents)
        {
            agent.GetNearbyObjects(neighbourRadius);
            agent.GetNearbyPredators(targetRadius);

            if(!this.transform.CompareTag("Predator"))
                if (agent.PredatorList.Count > 0 && this.flockState == FlockState.Chillin)
                {
                    // For now, get the first one on the list
                    predator = agent.PredatorList[0].gameObject;
                    AlertFlockOfPredatorPresence();
                }

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

    public void ChangeFlockSpeed(float newSpeed)
    {
        this.maxSpeed = newSpeed;
        this.squareMaxSpeed = this.maxSpeed * this.maxSpeed;
    }

    public void SetBehaviour(FlockBehaviour behaviour)
    {
        this.behaviour = behaviour;
    }

    public void ChangeNeighbourRadius(float newRadius)
    {
        this.squareNeighbourRadius = this.neighbourRadius * this.neighbourRadius;
        SquareAvoidanceRadius = this.squareNeighbourRadius * this.avoidanceRadiusMultiplier * this.avoidanceRadiusMultiplier;
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

    public void ChangeFlockState(FlockState newState) => this.flockState = newState;
    public float GetFlockSpeed() => maxSpeed;
    public float GetNeighbourRadius() => this.neighbourRadius;
    public List<FlockAgent> GetAgents() => this.agents;
    public GameObject GetCurrentPredator() => this.predator;
    public FlockBehaviour GetFlockBehaviour() => this.behaviour;

    void GoBackToChillin()
    {
        this.flockState = FlockState.Chillin;
        animator.SetTrigger("isChillin");
    }

    void AlertFlockOfPredatorPresence()
    {
        this.flockState = FlockState.PredatorPresence;
        animator.SetTrigger("isPredatorPresent");
    }

    void AlertFlockOfPredatorChase()
    {
        this.flockState = FlockState.PredatorChase;
        animator.SetTrigger("isPredatorChasing");
    }

    void AlertFlockOfPredatorAttack()
    {
        this.flockState = FlockState.PredatorAttack;
        animator.SetTrigger("isPredatorAttacking");
    }
}
