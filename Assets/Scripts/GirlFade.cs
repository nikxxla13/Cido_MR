using UnityEngine;

public class GirlFade : MonoBehaviour
{
    [Header("Fade Settings")]
    public int totalFruits = 10;
    public float minAlpha = 0f;
    public AudioClip reactionSound;

    private int delivered = 0;
    private Renderer[] renderers;
    private AudioSource audioSource;

    void Start()
    {
        renderers   = GetComponentsInChildren<Renderer>();
        audioSource = GetComponent<AudioSource>();

        foreach (var r in renderers)
            foreach (var m in r.materials)
                SetMaterialTransparent(m);
    }

    public void OnFruitDelivered()
    {
        delivered++;
        float alpha = Mathf.Lerp(1f, minAlpha, (float)delivered / totalFruits);
        ApplyAlpha(alpha);

        if (reactionSound != null && audioSource != null)
            audioSource.PlayOneShot(reactionSound);

        if (delivered >= totalFruits)
            FindObjectOfType<DifficultyManager>()?.OnExperienceComplete();
    }

    void ApplyAlpha(float alpha)
    {
        foreach (var r in renderers)
            foreach (var m in r.materials)
            {
                Color c = m.color;
                c.a = alpha;
                m.color = c;
            }
    }

    void SetMaterialTransparent(Material m)
    {
        m.SetFloat("_Surface", 1);
        m.SetFloat("_Blend", 0);
        m.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
        m.renderQueue = 3000;
    }
}
