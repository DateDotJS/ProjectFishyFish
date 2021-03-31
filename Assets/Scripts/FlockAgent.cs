using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FlockAgent : MonoBehaviour
{
    public Flock AgentFlock { get; set; }
    public Collider AgentCollider { get; set; }

    public List<Transform> Context { get; set; }

    void Start()
    {
        AgentCollider = GetComponent<Collider>();
        Context = new List<Transform>();
    }

    public void Move(Vector3 velocity)
    {
        transform.forward = velocity;
        transform.position += velocity * Time.deltaTime;
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
