using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Herd Behaviour")]
public class HerdBehaviour : FilteredFlockBehaviour
{
    private bool speedChanged = false;
    [SerializeField] private float speedFactor;

    public override Vector3 CalculateFilteredMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        //No predator in sight
        if (agent.PredatorList.Count == 0)
        {
            //Reset speed to initial one
            if (speedChanged == true)
            {
                flock.ChangeFlockSpeed(flock.GetFlockSpeed() / speedFactor);
                speedChanged = false;
            }

            return Vector3.zero;
        }

        Vector3 herdMove = Vector3.zero;

        Transform predator = agent.PredatorList[0].transform;
        float angle = Vector3.SignedAngle(predator.forward, agent.transform.position - predator.position, Vector3.up);
        
       if ( (angle > 0 && angle < 45) || (angle < 0 && angle > -45) )
            if (speedChanged == false)
            {
                flock.ChangeFlockSpeed(flock.GetFlockSpeed() * speedFactor);
                speedChanged = true;
            }
        else if (angle > 45 && angle < 90)
                herdMove = agent.transform.position + agent.transform.right * Time.deltaTime * 0.3f;
        else if (angle < -45 && angle > -90)
                herdMove = agent.transform.position - agent.transform.right * Time.deltaTime * 0.3f;
        
        return herdMove;
    }
}
