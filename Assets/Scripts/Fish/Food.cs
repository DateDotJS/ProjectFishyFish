using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public float Size;
    public float Weight;
    public int DangerLevel;
    public List<string> Composition; // e.g. poisons components, etc

    public FishType FishType;

    public bool IsRelativePreyToSelf(Food food)
    {
        return this.DangerLevel >= food.DangerLevel; 
        // We can extend this method later on
    }

    public bool IsRelativePredatorToSelf(Food food)
    {
        return food.DangerLevel >= this.DangerLevel;
        // We can extend this method later on
    }

}
