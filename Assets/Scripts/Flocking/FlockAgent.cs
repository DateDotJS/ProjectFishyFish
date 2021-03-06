using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FlockAgent : MonoBehaviour
{
    public Flock AgentFlock { get; set; }
    public Collider AgentCollider { get; set; }

    public List<Transform> Context { get; set; }
    
    public List<Transform> PredatorList { get; set; }
    
    public Kinematic Kinematics;

    [SerializeField] private float obstacleUpdateDelay = 1;
    private float obstacleUpdateTimer;
    static private int obstacleLayerMask;
    public int ObstacleLayerMask => obstacleLayerMask;
    static private int fishLayerMask;

    [SerializeField] private float rotationSpeed;

    private Animator animator;

    void Start()
    {
        AgentCollider = GetComponent<Collider>();
        Context = new List<Transform>();
        PredatorList = new List<Transform>();
        animator = GetComponent<Animator>();

        Kinematics = new Kinematic();

        obstacleUpdateTimer = obstacleUpdateDelay;
        obstacleLayerMask = LayerMask.GetMask("Obstacle");
        fishLayerMask = LayerMask.GetMask("Fish");
    }

    public void Move()
    {
        transform.forward = Vector3.Slerp(transform.forward, Kinematics.LinearVel, Time.deltaTime * rotationSpeed);
        transform.position += Kinematics.LinearVel * Time.deltaTime;

        // Animate fish swimming
        animator.SetFloat("SwimVelocity", Kinematics.LinearVel.magnitude);
    }


    public void GetNearbyObjects(float neighbourRadius)
    {
        Context.Clear();

        List<Collider> contextColliders = AgentFlock.GetAgentsWithinRange(this, transform.position, neighbourRadius);

        obstacleUpdateTimer += Time.deltaTime;
        if(obstacleUpdateTimer >= obstacleUpdateDelay) {
            obstacleUpdateTimer = Time.deltaTime;
            Collider[] colliders = Physics.OverlapSphere(transform.position, neighbourRadius, obstacleLayerMask);
            contextColliders.AddRange(colliders);
        } 

        foreach (var collider in contextColliders)
        {
            if (collider == AgentCollider)
                continue;

            if (collider.transform.parent == transform.parent)
            {
                Context.Add(collider.transform);
            }
        }
    }

    public void DestroyAgent()
    {
        AgentFlock.RemoveAgent(this);
        Destroy(gameObject);
    }

    /// <summary>
    /// Stores nearby predators to the PredatorList
    /// </summary>
    /// <param name="predatorRadius"></param>
    public void GetNearbyPredators(float predatorRadius)
    {
        PredatorList.Clear();
        
        var targetColliders = Physics.OverlapSphere(transform.position, predatorRadius, fishLayerMask);

        foreach (var collider in targetColliders)
        {
            if (collider == AgentCollider)
                continue;

            //Currently checking for object with Predator tag
            Transform other = collider.transform;
            if (other.CompareTag("Predator") || other.CompareTag("Player"))
            {
                PredatorList.Add(collider.transform);
            }
        }
    }
}
