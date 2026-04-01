using UnityEngine;

public class FruitSway : MonoBehaviour
{
    public float speed = 1f;
    public float swayAmount = 15f;

    private Vector3 startRotation;
    private float offset;

    void Start()
    {
        startRotation = transform.localEulerAngles;
        offset        = Random.Range(0f, Mathf.PI * 2f);
    }

    void Update()
    {
        var fruitHit = GetComponent<FruitHit>();
        if (fruitHit != null && fruitHit.detached) return;

        float sway = Mathf.Sin(Time.time * speed + offset) * swayAmount;
        transform.localEulerAngles = startRotation + new Vector3(0f, 0f, sway);
    }
}
