using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.Input;

public class StoneThrow : MonoBehaviour
{
    [Header("Throw Settings")]
    public float minThrowForce = 1f;
    public float maxThrowForce = 20f;
    public float chargeRate = 5f;

    private Rigidbody rb;
    private bool isHeld = false;
    private float chargeAmount = 0f;
    private OVRInput.Controller holdingController = OVRInput.Controller.None;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        gameObject.tag = "Stone";
    }

    void Update()
    {
        if (!isHeld) return;

        bool rightGrip = OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch);
        bool leftGrip  = OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch);

        if (rightGrip || leftGrip)
            chargeAmount = Mathf.Clamp(chargeAmount + chargeRate * Time.deltaTime, minThrowForce, maxThrowForce);
    }

    public void OnGrabbed(OVRInput.Controller controller)
    {
        isHeld = true;
        chargeAmount = minThrowForce;
        holdingController = controller;
        rb.isKinematic = true;
    }

    public void OnReleased()
    {
        isHeld = false;
        rb.isKinematic = false;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        Vector3 velocity  = OVRInput.GetLocalControllerVelocity(holdingController);
        Vector3 throwDir  = velocity.normalized;

        if (throwDir == Vector3.zero)
            throwDir = transform.forward;

        rb.AddForce(throwDir * chargeAmount, ForceMode.Impulse);
        chargeAmount = minThrowForce;
        holdingController = OVRInput.Controller.None;
    }
}
