using UnityEngine;

public class FruitHit : MonoBehaviour
{
    [Header("Hit Settings")]
    public int hitsRequired = 1;
    public string fruitType = "Apple";
    public bool detached = false;

    private int hitCount = 0;

    void OnCollisionEnter(Collision col)
    {
        if (detached) return;
        if (!col.gameObject.CompareTag("Stone")) return;

        hitCount++;
        StartCoroutine(ShakeEffect());

        if (hitCount >= hitsRequired)
            Detach();
    }

    public void Detach()
    {
        if (detached) return;
        detached = true;

        transform.SetParent(null);
        var rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true;

        GetComponent<FruitBalance>()?.SetActive(true);
    }

    System.Collections.IEnumerator ShakeEffect()
    {
        Vector3 originPos = transform.localPosition;
        for (int i = 0; i < 6; i++)
        {
            transform.localPosition = originPos + Random.insideUnitSphere * 0.02f;
            yield return new WaitForSeconds(0.04f);
        }
        transform.localPosition = originPos;
    }
}
