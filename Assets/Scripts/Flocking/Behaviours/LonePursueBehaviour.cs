using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Lone Pursue")]
public class LonePursueBehaviour : FlockBehaviour
{
    [SerializeField] private float maxSpeed;
    [SerializeField] private float radius;
    [SerializeField] private float timeToTarget = 0.25f;

    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        Vector3 direction = ((LoneFlock)flock).GetTarget().transform.position - agent.transform.position;
        Vector3 velocity = new Vector3(0, 0, 0);
        Debug.Log(((LoneFlock)flock).GetTarget().transform.position);
        if (direction.magnitude < radius) {
            return velocity;
        }

        velocity = direction / timeToTarget;
        if(velocity.magnitude > maxSpeed) {
            velocity = velocity.normalized * maxSpeed;
        }

        return velocity;
    }
}
