using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class keeps track of a body of water that has a
/// current. If a fish enters this body of water, their
/// velocity would be affected by this class.
///
/// Note: The direction of this current is depicted by the forward vector (z).
///         Therefore, you'll need to rotate this game object to change its direction.
/// </summary>
public class WaterCurrent : MonoBehaviour
{
    [Tooltip("This specifies the speed of the velocity. To change the direction of it, you will need to change this" +
             " game object's rotation.")]
    [SerializeField] private float speed;
    public Vector3 Velocity { get; private set; }
    
    private void Awake()
    {
        Velocity = transform.forward * speed;
    }

    private void OnDrawGizmosSelected() => Gizmos.DrawLine(transform.position, transform.forward * speed);
}
