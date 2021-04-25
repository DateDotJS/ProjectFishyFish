using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MainPlayer : MonoBehaviour
{
    private PlayerController playerController;

    public List<FishTypeRequirement> consumedFishAmounts;

    [SerializeField] private List<EvolutionMilestone> milestones;

    private List<FishScore> fishscores;
    private Food food;

    private Text milestoneDisplay;

    private void Awake()
    {
        this.playerController = GetComponent<PlayerController>();
        this.food = GetComponent<Food>();
        this.milestoneDisplay = GameObject
            .FindGameObjectWithTag("MilestoneAnnouncement")
            .GetComponent<Text>();

        this.consumedFishAmounts = new List<FishTypeRequirement>();

        // comment out if we decide to save in between game session
        foreach (var milstone in this.milestones)
            milstone.HasActivated = false;

        this.fishscores = GameObject.FindGameObjectsWithTag("FishScore")
            .Select(go => go.GetComponent<FishScore>())
            .ToList();
    }

    private void Start()
    {
        this.milestoneDisplay.enabled = false;
    }

    private void Update()
    {
        // FOR NOW
        if (Input.GetKeyDown(KeyCode.F))
            DEBUGConsumeREDFish();

        foreach (var milestone in this.milestones)
            milestone.TryApplyMilestone(this);
    }

    public void DEBUGConsumeREDFish()
    {
        var entry = this.consumedFishAmounts
            .Where(entry => entry.FishType == FishType.RedFish)
            .FirstOrDefault();

        if (entry is null)
        {
            entry = new FishTypeRequirement
            {
                FishType = FishType.RedFish,
                Amount = 0,
            };

            this.consumedFishAmounts.Add(entry);
        }
        
        entry.Amount += 1;

        var correspondingScore = this.fishscores
            .Where(score => score.FishType == entry.FishType)
            .FirstOrDefault();

        if (correspondingScore is null)
        {
            Debug.LogError($"Need a corresponding score of type {entry.FishType}!");
        } else
        {
            correspondingScore.SetScore(entry.Amount);
        }
    }

    public void ConsumeFish(Fish fish)
    {
        var entry = this.consumedFishAmounts
            .Where(entry => entry.FishType == fish.FishType)
            .FirstOrDefault();

        if (entry is null)
            this.consumedFishAmounts.Add(new FishTypeRequirement
            {
                FishType = fish.FishType
            });

        entry.Amount += 1;
    }


    public void UpdateSpeedByPercent(float speedIncreasePercent)
    {
        this.playerController.MinForwardSpeed *= 1f + speedIncreasePercent;
        this.playerController.MaxForwardSpeed *= 1f + speedIncreasePercent;
    }

    public void UpdateSizeByPercent(float sizeIncreasePercent)
    {
        this.transform.localScale *= 1f + sizeIncreasePercent;
    }

    public void EvolutionUpdate()
    {
        throw new System.NotImplementedException();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("FishFlocker"))
            CollideWithFlockAgent(collision.gameObject.GetComponent<FlockAgent>());
    }

    private void CollideWithFlockAgent(FlockAgent flockAgent)
    {
        var agentAsFood = flockAgent.GetComponent<Food>();

        if (this.food.IsRelativePreyToSelf(agentAsFood))
            EatFish(flockAgent, agentAsFood);
        
    }

    private void EatFish(FlockAgent flockAgent, Food agentAsFood)
    {
        var entry = this.consumedFishAmounts
            .Where(entry => entry.FishType == agentAsFood.FishType)
            .FirstOrDefault();

        if (entry is null)
        {
            entry = new FishTypeRequirement
            {
                FishType = agentAsFood.FishType,
                Amount = 0,
            };

            this.consumedFishAmounts.Add(entry);
        }

        entry.Amount += 1;

        var correspondingScore = this.fishscores
            .Where(score => score.FishType == entry.FishType)
            .FirstOrDefault();

        if (correspondingScore is null)
            Debug.LogError($"Need a corresponding score of type {entry.FishType}!");
        else
            correspondingScore.SetScore(entry.Amount);

        flockAgent.DestroyAgent();
    }

    public void DisplayMilestoneMessage(string message)
    {
        this.milestoneDisplay.enabled = true;
        this.milestoneDisplay.text = message;
        
        StartCoroutine(HideMilestoneMessageIn(2f));
    }

    private IEnumerator HideMilestoneMessageIn(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        this.milestoneDisplay.enabled = false;

        yield return null;
    }
}
