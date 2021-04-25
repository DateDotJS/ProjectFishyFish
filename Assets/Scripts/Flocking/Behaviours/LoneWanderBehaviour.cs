using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Lone Wander")]
public class LoneWanderBehaviour : FlockBehaviour
{
    [SerializeField] private float wanderVariation;
    [SerializeField] private float speed;
    [SerializeField] private float distance;
    [SerializeField] private float radius = 0.1f;

    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        LoneFlock loneFlock = (LoneFlock)flock;

        if(loneFlock.SetWanderTimer(Time.deltaTime)) {
            loneFlock.SetWanderStart(agent.Kinematics.LinearVel);
            Vector3 variation = new Vector3();
            variation.x = Random.Range(-wanderVariation, wanderVariation);
            variation.y = Random.Range(-wanderVariation, wanderVariation);
            variation.z = Random.Range(-wanderVariation, wanderVariation);
            loneFlock.SetWanderVariation(variation);
        }

        Vector3 velocity = Vector3.Lerp(loneFlock.GetWanderStart(), agent.Kinematics.LinearVel + loneFlock.GetWanderVariation(), loneFlock.GetWanderTimer()/loneFlock.GetWanderDelay());

        return velocity;
    }
}
