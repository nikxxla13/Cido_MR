using UnityEngine;

public class DeliveryZone : MonoBehaviour
{
    private FruitSequence sequenceManager;
    private GirlFade girlFade;

    void Start()
    {
        sequenceManager = FindObjectOfType<FruitSequence>();
        girlFade        = FindObjectOfType<GirlFade>();
    }

    void OnTriggerEnter(Collider other)
    {
        var fruitHit = other.GetComponent<FruitHit>();
        if (fruitHit == null || !fruitHit.detached) return;

        var grabbable = other.GetComponent<OVRGrabbable>();
        if (grabbable == null || !grabbable.isGrabbed) return;

        sequenceManager?.DeliverFruit(fruitHit.fruitType);
        girlFade?.OnFruitDelivered();
        Destroy(other.gameObject, 0.1f);
    }
}
