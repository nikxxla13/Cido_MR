using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class FruitSequence : MonoBehaviour
{
    [Header("Sequence Settings")]
    public List<string> requiredSequence = new List<string>();
    public float timeLimit = 20f;

    [Header("UI")]
    public TextMeshProUGUI sequenceDisplay;
    public TextMeshProUGUI timerDisplay;

    private int currentIndex = 0;
    private float timer = 0f;
    private bool sequenceActive = false;

    public static FruitSequence Instance;

    void Awake() => Instance = this;

    void Start()
    {
        if (requiredSequence.Count > 0)
            StartSequence();
    }

    public void StartSequence()
    {
        currentIndex   = 0;
        timer          = timeLimit;
        sequenceActive = true;
        UpdateUI();
    }

    public void SetSequence(List<string> sequence, float timeLimitOverride = -1)
    {
        requiredSequence = sequence;
        if (timeLimitOverride > 0) timeLimit = timeLimitOverride;
        StartSequence();
    }

    void Update()
    {
        if (!sequenceActive) return;

        timer -= Time.deltaTime;

        if (timerDisplay != null)
            timerDisplay.text = Mathf.CeilToInt(timer).ToString();

        if (timer <= 0f)
            ResetSequence();
    }

    public void DeliverFruit(string fruitType)
    {
        if (!sequenceActive || requiredSequence.Count == 0)
        {
            FindObjectOfType<GirlFade>()?.OnFruitDelivered();
            return;
        }

        if (fruitType == requiredSequence[currentIndex])
        {
            currentIndex++;
            UpdateUI();
            if (currentIndex >= requiredSequence.Count)
                SequenceComplete();
        }
        else
        {
            ResetSequence();
        }
    }

    void SequenceComplete()
    {
        sequenceActive = false;
        FindObjectOfType<DifficultyManager>()?.OnStageComplete();
        if (sequenceDisplay != null) sequenceDisplay.text = "Well done!";
    }

    void ResetSequence()
    {
        currentIndex = 0;
        timer        = timeLimit;
        UpdateUI();
        FindObjectOfType<TreeManager>()?.RefreshTree();
    }

    void UpdateUI()
    {
        if (sequenceDisplay == null) return;
        string display = "Give me: ";
        for (int i = 0; i < requiredSequence.Count; i++)
        {
            if (i == currentIndex)       display += $"<b>[{requiredSequence[i]}]</b> ";
            else if (i < currentIndex)   display += $"<s>{requiredSequence[i]}</s> ";
            else                         display += $"{requiredSequence[i]} ";
        }
        sequenceDisplay.text = display;
    }
}
