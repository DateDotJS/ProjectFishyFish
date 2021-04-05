using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FlockAgent : MonoBehaviour
{
    public Flock AgentFlock { get; set; }
    public Collider AgentCollider { get; set; }

    public List<Transform> Context { get; set; }
    
    public Kinematic Kinematics;
    
    void Start()
    {
        AgentCollider = GetComponent<Collider>();
        Context = new List<Transform>();
        
        Kinematics = new Kinematic();
    }

    public void Move()
    {
        transform.forward = Kinematics.LinearVel;
        transform.position += Kinematics.LinearVel * Time.deltaTime;
    }


    public void GetNearbyObjects(float neighbourRadius)
    {
        Context.Clear();

        var contextColliders = Physics.OverlapSphere(transform.position, neighbourRadius);

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


}
