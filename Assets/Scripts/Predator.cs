using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LoneFlock))]
[RequireComponent(typeof(Food))]
public class Predator : MonoBehaviour
{
    [SerializeField] private float sightUpdateDelay;
    [SerializeField] private float sightRadius;
    [SerializeField] private float targetRadius;
    [SerializeField] private float blindSpotAngle;
    [SerializeField] private float pursueDelay;
    [SerializeField] private int maxObservablePreys;
    [SerializeField] private FlockBehaviour pursueBehaviour;
    [SerializeField] private FlockBehaviour wanderBehaviour;

    private Food food;

    private readonly string FISH_LAYER = "Fish";
    static private int fishLayerMask;

    private float sightTimer;
    private float pursueTimer;
    private bool isOnPursueBreak;

    private Collider[] observedFish;
    private int nbObservedFish;
    private int targetedPrey;

    private float blindSpotThreshold;

    private LoneFlock flock;
    private FlockAgent self;

    private void Awake()
    {
        food = GetComponent<Food>();

        fishLayerMask = LayerMask.GetMask("Fish");

        sightTimer = sightUpdateDelay;
        pursueTimer = 0;
        isOnPursueBreak = false;

        observedFish = new Collider[maxObservablePreys];
        nbObservedFish = 0;
        targetedPrey = 0;

        blindSpotThreshold = 180.0f - (blindSpotAngle / 2);

        flock = GetComponent<LoneFlock>();
        flock.SetBehaviour(wanderBehaviour);
        self = GetComponent<FlockAgent>();
    }

    private void FixedUpdate()
    {
        // Look for fishes
        sightTimer += Time.fixedDeltaTime;

        if(observedFish[targetedPrey] == null && flock.GetFlockBehaviour() == pursueBehaviour) {
            flock.SetBehaviour(wanderBehaviour);
            return;
        }

        if(isOnPursueBreak) {
            pursueTimer += Time.fixedDeltaTime;

            if(pursueTimer >= pursueDelay) {
                isOnPursueBreak = false;
            }
        }

        if (ChooseTarget() && !isOnPursueBreak) {
            flock.SetTarget(observedFish[targetedPrey].gameObject);
            flock.SetBehaviour(pursueBehaviour);
        } else {
            flock.SetBehaviour(wanderBehaviour);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, sightRadius);

        // Draw blindspot
        //Gizmos.color = Color.green;
        //float x = sightRadius * Mathf.Cos((blindSpotAngle / 2) * Mathf.Deg2Rad) + transform.forward.x;
        //float y = sightRadius * Mathf.Sin((blindSpotAngle / 2) * Mathf.Deg2Rad) + transform.forward.y;
        //float z = transform.forward.z - sightRadius;
        //Gizmos.DrawLine(transform.position, new Vector3(x/2, y, z));
        //Gizmos.DrawLine(transform.position, new Vector3(-x/2, y, z));
        // Gizmos.DrawLine(transform.position, new Vector3(x/2, -y, z));
        //Gizmos.DrawLine(transform.position, new Vector3(-x/2, -y, z));
        //Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, y, z));
        //Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, -y, z));
    }

    /* Chooses the closest valid Prey target.
     * Returns true if a target was found, false otherwise.
     */
    private bool ChooseTarget()
    {
        if (sightTimer >= sightUpdateDelay) {
            sightTimer = 0.0f;

            nbObservedFish = Physics.OverlapSphereNonAlloc(transform.position, sightRadius, observedFish, fishLayerMask);
        }

        if(observedFish[targetedPrey] != null
            && observedFish[targetedPrey].gameObject.CompareTag("FishFlocker")
            && Vector3.Distance(observedFish[targetedPrey].transform.position, transform.position) <= targetRadius) {
            return true;
        }

        int chosenTarget = -1;
        float minDistance = Mathf.Infinity;

        for (int i = 0; i < nbObservedFish; i++) {
            if (observedFish[i] != null && observedFish[i].gameObject.CompareTag("FishFlocker")) {
                Vector3 preyPosition = observedFish[i].transform.position;
                if (!IsPositionInBlindSpot(preyPosition)) {
                    float distance = Vector3.Distance(transform.position, preyPosition);
                    if (distance < minDistance) {
                        chosenTarget = i;
                        minDistance = distance;
                    }
                }
            }
        }

        if (chosenTarget == -1 || minDistance == Mathf.Infinity) {
            return false;
        } else {
            targetedPrey = chosenTarget;
            return true;
        }
    }

    private bool IsPositionInBlindSpot(Vector3 position)
    {
        float anglePredatorToPosition = Vector3.Angle(transform.forward, position);
        return anglePredatorToPosition >= blindSpotThreshold;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("FishFlocker"))
            CollideWithFlockAgent(collision.gameObject.GetComponent<FlockAgent>());
    }

    private void CollideWithFlockAgent(FlockAgent flockAgent)
    {
        var agentAsFood = flockAgent.GetComponent<Food>();

        if (food.IsRelativePreyToSelf(agentAsFood))
            EatFish(flockAgent, agentAsFood);

    }

    private void EatFish(FlockAgent flockAgent, Food agentAsFood)
    {
        flockAgent.DestroyAgent();
        sightTimer = sightUpdateDelay;
        targetedPrey = 0;
        isOnPursueBreak = true;
        pursueTimer = 0;
        flock.SetBehaviour(wanderBehaviour);
    }
}
