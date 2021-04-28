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
    [SerializeField] private ParticleSystem eatingBubbles;

    private List<FishScore> fishscores;
    private Food food;

    private Text milestoneDisplay;
    private Text milestoneSubDisplay;
    private float milestoneDisplayTimer;
    private bool milestoneDisplayIsDisplayed;

    [SerializeField] private float timeToDisplayMilestoneMessage = 4f;
    
    [SerializeField] private bool debugConsumeFish = false;

    private void Awake()
    {
        this.playerController = GetComponent<PlayerController>();
        this.food = GetComponent<Food>();
        this.milestoneDisplay = GameObject
            .FindGameObjectWithTag("MilestoneAnnouncement")
            .GetComponent<Text>();
        this.milestoneSubDisplay = GameObject
            .FindGameObjectWithTag("MilestoneSub")
            .GetComponent<Text>();

        this.consumedFishAmounts = new List<FishTypeRequirement>();

        foreach (var milstone in this.milestones)
            milstone.HasActivated = false;

        this.fishscores = GameObject.FindGameObjectsWithTag("FishScore")
            .Select(go => go.GetComponent<FishScore>())
            .ToList();
    }

    private void Start()
    {
        this.milestoneDisplay.enabled = false;
        this.milestoneSubDisplay.enabled = false;
    }

    private void Update()
    {
        if (this.debugConsumeFish)
            AcceptDebugConsumeFishes();

        foreach (var milestone in this.milestones)
            milestone.TryApplyMilestone(this);
    }

    private void AcceptDebugConsumeFishes()
    {
        if (Input.GetKeyDown(KeyCode.Alpha7))
            DebugConsumeAFish(FishType.BlueFish);
        if (Input.GetKeyDown(KeyCode.Alpha8))
            DebugConsumeAFish(FishType.GreenFish);
        if (Input.GetKeyDown(KeyCode.Alpha9))
            DebugConsumeAFish(FishType.OrangeFish);
        if (Input.GetKeyDown(KeyCode.Alpha0))
            DebugConsumeAFish(FishType.RedFish);
    }

    public void DebugConsumeAFish(FishType fishType)
    {
        var entry = this.consumedFishAmounts
            .Where(entry => entry.FishType == fishType)
            .FirstOrDefault();

        if (entry is null)
        {
            entry = new FishTypeRequirement
            {
                FishType = fishType,
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

    public void UpdateSpeedByPercent(float speedIncreasePercent)
    {
        this.playerController.MinForwardSpeed *= 1f + speedIncreasePercent;
        this.playerController.MaxForwardSpeed *= 1f + speedIncreasePercent;
    }

    public void UpdateSizeByPercent(float sizeIncreasePercent)
    {
        this.transform.localScale *= 1f + sizeIncreasePercent;
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
        PlayEatingFX();
    }

    public void DisplayMilestoneMessage(string message, string subMessage)
    {
        this.milestoneDisplay.enabled = true;
        this.milestoneDisplay.text = message;

        this.milestoneSubDisplay.enabled = true;
        this.milestoneSubDisplay.text = subMessage;

        this.milestoneDisplayTimer = this.timeToDisplayMilestoneMessage;

        if (!this.milestoneDisplayIsDisplayed)
            StartCoroutine(DelayedHideMilestoneMessage());

        this.milestoneDisplayIsDisplayed = true;
    }

    private IEnumerator DelayedHideMilestoneMessage()
    {
        while (this.milestoneDisplayTimer > 0)
        {
            this.milestoneDisplayTimer -= Time.deltaTime;
            yield return null;
        }

        this.milestoneDisplay.enabled = false;
        this.milestoneSubDisplay.enabled = false;

        this.milestoneDisplayIsDisplayed = false;

        yield return null;
    }

    private void PlayEatingFX()
    {
        if (eatingBubbles != null && !eatingBubbles.isPlaying)
            eatingBubbles.Play();
    }
}
