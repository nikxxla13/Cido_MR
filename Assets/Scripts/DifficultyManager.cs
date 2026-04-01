using UnityEngine;
using System.Collections.Generic;

public class DifficultyManager : MonoBehaviour
{
    [Header("Stage Config")]
    public int currentStage = 0;
    public int totalStages = 6;

    [System.Serializable]
    public class StageSettings
    {
        public string stageName;
        public int hitsRequired = 1;
        public float hookAttachTime = 1.5f;
        public float balanceShiftInterval = 3f;
        public float balanceCorrectionWindow = 1.5f;
        public float balanceDifficulty = 1f;
        public List<string> fruitSequence = new List<string>();
        public float sequenceTimeLimit = 20f;
        public bool fruitsMove = false;
        public float fruitSwaySpeed = 0f;
    }

    public List<StageSettings> stages = new List<StageSettings>();

    private FruitSequence sequenceManager;
    private TreeManager treeManager;

    void Start()
    {
        sequenceManager = GetComponent<FruitSequence>();
        treeManager     = FindObjectOfType<TreeManager>();

        if (stages.Count == 0) BuildDefaultStages();
        ApplyStage(currentStage);
    }

    public void OnStageComplete()
    {
        currentStage++;
        if (currentStage >= totalStages) { OnExperienceComplete(); return; }
        ApplyStage(currentStage);
    }

    public void OnExperienceComplete()
    {
        Debug.Log("Experience complete!");
    }

    void ApplyStage(int index)
    {
        if (index >= stages.Count) return;
        StageSettings s = stages[index];

        foreach (var fruit in FindObjectsOfType<FruitHit>())
            fruit.hitsRequired = s.hitsRequired;

        foreach (var hook in FindObjectsOfType<HookMechanic>())
            hook.attachTime = s.hookAttachTime;

        foreach (var balance in FindObjectsOfType<FruitBalance>())
        {
            balance.shiftInterval     = s.balanceShiftInterval;
            balance.correctionWindow  = s.balanceCorrectionWindow;
            balance.SetDifficulty(s.balanceDifficulty);
        }

        if (s.fruitSequence.Count > 0)
            sequenceManager?.SetSequence(s.fruitSequence, s.sequenceTimeLimit);

        treeManager?.SetStage(index, s.fruitsMove, s.fruitSwaySpeed);
    }

    void BuildDefaultStages()
    {
        stages = new List<StageSettings>
        {
            new StageSettings { stageName = "Stage 1", hitsRequired = 1, hookAttachTime = 2f,   balanceShiftInterval = 5f,   balanceCorrectionWindow = 2f,   balanceDifficulty = 0.5f, fruitSequence = new List<string>(),                                                    sequenceTimeLimit = 30f, fruitsMove = false },
            new StageSettings { stageName = "Stage 2", hitsRequired = 1, hookAttachTime = 1.5f, balanceShiftInterval = 4f,   balanceCorrectionWindow = 1.8f, balanceDifficulty = 0.8f, fruitSequence = new List<string> { "Apple", "Pear" },                                  sequenceTimeLimit = 25f, fruitsMove = false },
            new StageSettings { stageName = "Stage 3", hitsRequired = 2, hookAttachTime = 1.5f, balanceShiftInterval = 3f,   balanceCorrectionWindow = 1.5f, balanceDifficulty = 1f,   fruitSequence = new List<string> { "Apple", "Pear", "Apple" },                         sequenceTimeLimit = 20f, fruitsMove = true, fruitSwaySpeed = 0.5f },
            new StageSettings { stageName = "Stage 4", hitsRequired = 2, hookAttachTime = 1.2f, balanceShiftInterval = 2.5f, balanceCorrectionWindow = 1.2f, balanceDifficulty = 1.3f, fruitSequence = new List<string> { "Pear", "Apple", "Orange", "Pear" },                sequenceTimeLimit = 18f, fruitsMove = true, fruitSwaySpeed = 1f },
            new StageSettings { stageName = "Stage 5", hitsRequired = 3, hookAttachTime = 1f,   balanceShiftInterval = 2f,   balanceCorrectionWindow = 1f,   balanceDifficulty = 1.6f, fruitSequence = new List<string> { "Apple", "Orange", "Pear", "Apple", "Orange" },     sequenceTimeLimit = 15f, fruitsMove = true, fruitSwaySpeed = 1.5f },
            new StageSettings { stageName = "Stage 6", hitsRequired = 3, hookAttachTime = 0.8f, balanceShiftInterval = 1.5f, balanceCorrectionWindow = 0.8f, balanceDifficulty = 2f,   fruitSequence = new List<string> { "Orange", "Apple", "Pear", "Orange", "Apple", "Pear" }, sequenceTimeLimit = 12f, fruitsMove = true, fruitSwaySpeed = 2f },
        };
    }
}
