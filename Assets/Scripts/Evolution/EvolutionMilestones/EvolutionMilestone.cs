using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class FishTypeRequirement
{
    public FishType FishType;
    public int Amount;
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Evolution/Milestones/BasicMilestone", order = 1)]
public class EvolutionMilestone : ScriptableObject
{
    public List<FishTypeRequirement> Requirements;
    public bool HasActivated;

    public List<EvolutionMilestoneEffect> Effects;

    public string MilestoneMessage; 
    public string MilestoneSubMessage; 

    public void TryApplyMilestone(MainPlayer mainPlayer)
    {
        var inventory = mainPlayer.consumedFishAmounts;

        if (HasActivated)
            return;

        var meetsRequirements = CheckIfMeetsRequirements(inventory);

        if (!meetsRequirements)
        {
            //Debug.Log("Requirements not met");
            return;
        }

        //Debug.Log("Requirements met!");
        mainPlayer.DisplayMilestoneMessage(MilestoneMessage, MilestoneSubMessage);

        foreach (var effect in Effects)
            effect.ApplyTo(mainPlayer);

        HasActivated = true;
    }

    private bool CheckIfMeetsRequirements(List<FishTypeRequirement> inventory)
    {
        if (HasActivated)
            return false;

        foreach (var requirement in Requirements)
        {
            var correspondingEntry = inventory
                .Where(entry => entry.FishType == requirement.FishType)
                .FirstOrDefault();

            if (correspondingEntry is null)
                return false;

            if (correspondingEntry.Amount < requirement.Amount)
                return false;
        }

        return true;
    }
}