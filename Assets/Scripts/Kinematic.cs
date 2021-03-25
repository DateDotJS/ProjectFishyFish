using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kinematic
{
    //Instantaneous displacement
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
