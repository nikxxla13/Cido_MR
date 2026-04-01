using UnityEngine;

public class FruitStem : MonoBehaviour
{
    public void DetachFruit()
    {
        var fruitHit = GetComponentInParent<FruitHit>();
        if (fruitHit != null)
            fruitHit.Detach();
    }
}
