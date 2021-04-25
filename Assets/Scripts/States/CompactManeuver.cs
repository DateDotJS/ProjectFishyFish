using UnityEngine;

public class CompactManeuver : StateMachineBehaviour
{
    [SerializeField] private float distanceFactor;
    [SerializeField] private float speedFactor;

    private Flock flock;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        flock = animator.gameObject.GetComponent<Flock>();
        if (flock is null)
            return;
        
        // reduce the nearest neighbor distance
        flock.ChangeNeighbourRadius(flock.GetNeighbourRadius() / distanceFactor);
        // increase speed
        flock.ChangeFlockSpeed(flock.GetFlockSpeed() * this.speedFactor);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (flock is null)
            return;
        // reset back to normal speed
        flock.ChangeNeighbourRadius(flock.GetNeighbourRadius() * distanceFactor);
        flock.ChangeFlockSpeed(flock.GetFlockSpeed() / this.speedFactor);
    }
}
