using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EvolutionEffect/SpeedUp", order = 1)]
public class SpeedUp : EvolutionMilestoneEffect
{
    [Range(0.01f, 3f)]
    public float SpeedUpPercentage = 0.1f;

    public override void ApplyTo(MainPlayer mainPlayer)
    {
        mainPlayer.UpdateSpeedByPercent(SpeedUpPercentage);
    }
}

