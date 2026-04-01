using UnityEngine;

[RequireComponent(typeof(OVRGrabbable))]
public class FruitGrabbable : MonoBehaviour
{
    private OVRGrabbable grabbable;
    private FruitBalance balance;
    private StoneThrow stoneThrow;
    private bool wasGrabbed = false;

    void Awake()
    {
        grabbable  = GetComponent<OVRGrabbable>();
        balance    = GetComponent<FruitBalance>();
        stoneThrow = GetComponent<StoneThrow>();
    }

    void Update()
    {
        bool grabbed = grabbable.isGrabbed;

        if (grabbed && !wasGrabbed)
        {
            OVRInput.Controller ctrl = GetGrabbingController();
            balance?.OnGrabbed(ctrl);
            stoneThrow?.OnGrabbed(ctrl);
        }
        else if (!grabbed && wasGrabbed)
        {
            balance?.OnReleased();
            stoneThrow?.OnReleased();
        }

        wasGrabbed = grabbed;
    }

    OVRInput.Controller GetGrabbingController()
    {
        float distL = Vector3.Distance(transform.position, OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch));
        float distR = Vector3.Distance(transform.position, OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch));
        return distL < distR ? OVRInput.Controller.LTouch : OVRInput.Controller.RTouch;
    }
}
