using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EvolutionEffect/SizeUp", order = 2)]
public class SizeUp : EvolutionMilestoneEffect
{
    [Range(0.01f, 3f)]
    public float SizeUpPercentage = 0.1f;

    public override void ApplyTo(MainPlayer mainPlayer)
    {
        mainPlayer.UpdateSizeByPercent(SizeUpPercentage);
    }
}

