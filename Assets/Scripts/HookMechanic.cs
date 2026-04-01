using UnityEngine;
using UnityEngine.UI;

public class HookMechanic : MonoBehaviour
{
    [Header("Hook Settings")]
    public float attachTime = 1.5f;
    public float maxReach = 0.8f;

    [Header("UI")]
    public Image progressCircle;

    private float timer = 0f;
    private FruitStem currentStem = null;
    private Transform handAnchor;

    void Start()
    {
        var anchors = FindObjectsOfType<OVRHandPrefab>();
        if (anchors.Length > 0)
            handAnchor = anchors[0].transform;

        if (progressCircle != null)
            progressCircle.fillAmount = 0f;
    }

    void Update()
    {
        if (currentStem == null) return;

        if (handAnchor != null)
        {
            float dist = Vector3.Distance(transform.position, handAnchor.position);
            if (dist > maxReach)
            {
                ResetHook();
                return;
            }
        }

        timer += Time.deltaTime;

        if (progressCircle != null)
            progressCircle.fillAmount = timer / attachTime;

        if (timer >= attachTime)
        {
            currentStem.DetachFruit();
            ResetHook();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FruitStem") && currentStem == null)
            currentStem = other.GetComponent<FruitStem>();
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("FruitStem"))
            ResetHook();
    }

    void ResetHook()
    {
        timer = 0f;
        currentStem = null;
        if (progressCircle != null)
            progressCircle.fillAmount = 0f;
    }
}
