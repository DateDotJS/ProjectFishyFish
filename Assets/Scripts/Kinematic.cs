using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Kinematic serves as a custom data type for storing the movement and rotation of the agent
/// </summary>
public class Kinematic
{
    //Instantaneous velocity
    public Vector3 linearVel;
    public float angularVel;
    
    //Acceleration
    public float linearAcc;
    public float angularAcc;
    
    //Orientation in the world
    public float orientation;

    /// <summary>
    /// Default constructor
    /// </summary>
    public Kinematic()
    {
        linearVel = new Vector3();
        angularVel = 0.0f;
        linearAcc = 0.0f;
        angularAcc = 0.0f;
        orientation = 0.0f;
    }
}
