using UnityEngine;

public class JoinManeuver : StateMachineBehaviour
{
    private Flock flock;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        flock = animator.gameObject.GetComponent<Flock>();
        if (flock is null)
            return;

        CompositeBehaviour behaviour = flock.GetFlockBehaviour() as CompositeBehaviour;

        for (int i = 0; i < behaviour.Behaviours.Length; i++)
        {
            if (behaviour.Behaviours[i] is JoinBehaviour)
                behaviour.Weights[i] = 0.5f;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (flock is null)
            return;

        CompositeBehaviour behaviour = flock.GetFlockBehaviour() as CompositeBehaviour;

        for (int i = 0; i < behaviour.Behaviours.Length; i++)
        {
            if (behaviour.Behaviours[i] is JoinBehaviour)
                behaviour.Weights[i] = 0f;
        }

        flock.ChangeFlockState(FlockState.PredatorPresence);
    }
}
