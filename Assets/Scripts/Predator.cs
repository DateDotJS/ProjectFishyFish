using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Predator : MonoBehaviour
{
    [SerializeField]
    private float sightUpdateDelay;
    [SerializeField]
    private float sightRadius;
    [SerializeField]
    private float blindSpotAngle;
    [SerializeField]
    private int maxObservablePreys;
    [SerializeField]
    private GameObject testTarget;

    private readonly string FISH_LAYER = "Fish";
    private int fishLayerMask;

    private float sightTimer;

    private Collider[] observedFish;
    private int nbObservedFish;
    private int targetedPrey;

    private float blindSpotThreshold;

    private void Awake()
    {
        fishLayerMask = LayerMask.GetMask("Fish");

        sightTimer = sightUpdateDelay;

        observedFish = new Collider[maxObservablePreys];
        nbObservedFish = 0;
        targetedPrey = 0;

        blindSpotThreshold = 180.0f - (blindSpotAngle / 2);
    }

    private void FixedUpdate()
    {
        // Look for fishes
        sightTimer += Time.fixedDeltaTime;

        if(sightTimer >= sightUpdateDelay) {
            sightTimer = 0.0f;
            
            nbObservedFish = Physics.OverlapSphereNonAlloc(transform.position, sightRadius, observedFish, fishLayerMask);
        }

        if(ChooseTarget()) {
            // Move and look towards target
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, sightRadius);

        // Draw blindspot
        Gizmos.color = Color.green;
        float x = sightRadius * Mathf.Cos((blindSpotAngle / 2) * Mathf.Deg2Rad) + transform.position.x;
        float y = sightRadius * Mathf.Sin((blindSpotAngle / 2) * Mathf.Deg2Rad) + transform.position.y;
        float z = transform.position.z - sightRadius;
        Gizmos.DrawLine(transform.position, new Vector3(x/2, y, z));
        Gizmos.DrawLine(transform.position, new Vector3(-x/2, y, z));
        Gizmos.DrawLine(transform.position, new Vector3(x/2, -y, z));
        Gizmos.DrawLine(transform.position, new Vector3(-x/2, -y, z));
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, y, z));
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, -y, z));
    }

    /* Chooses the closest valid Prey target.
     * Returns true if a target was found, false otherwise.
     */
    private bool ChooseTarget()
    {
        int chosenTarget = -1;
        float minDistance = Mathf.Infinity;

        for (int i = 0; i < nbObservedFish; i++) {
            // TODO: Check if fish is a prey
            if(true) {
                Vector3 preyPosition = observedFish[i].transform.position;
                if (!IsPositionInBlindSpot(preyPosition)) {
                    print("visible");
                    float distance = Vector3.Distance(transform.position, preyPosition);
                    if(distance < minDistance) {
                        chosenTarget = i;
                        minDistance = distance;
                    }
                }
            }
        }

        if(chosenTarget == -1 || minDistance == Mathf.Infinity) {
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
}
