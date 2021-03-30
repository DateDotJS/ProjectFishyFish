using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Kinematic serves as a custom data type for storing the movement and rotation of the agent
/// </summary>
public class Kinematic
{
    [Header("Instantaneous Velocity")]
    public Vector3 LinearVel;
    public float AngularVel;
    
    [Header("Acceleration")]
    public float LinearAcc;
    public float AngularAcc;
    
    [Header("Orientation")]
    public float Orientation;
    
    public Kinematic()
    {
        LinearVel = new Vector3();
        AngularVel = 0;
        LinearAcc = 0;
        AngularAcc = 0;
        Orientation = 0;
    }
}
