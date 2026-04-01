using UnityEngine;

public class FruitBalance : MonoBehaviour
{
    [Header("Balance Settings")]
    public float shiftInterval = 3f;
    public float correctionWindow = 1.5f;
    public float tiltAngleRequired = 25f;
    public float instabilityScale = 1f;

    private bool active = false;
    private bool isHeld = false;
    private float timer = 0f;
    private bool waitingCorrection = false;
    private Vector3 requiredTiltDir = Vector3.zero;
    private OVRInput.Controller holdingController = OVRInput.Controller.None;

    private Vector3 baseLocalPos;

    void Start() => baseLocalPos = transform.localPosition;

    public void SetActive(bool state) => active = state;

    public void OnGrabbed(OVRInput.Controller controller)
    {
        isHeld = true;
        holdingController = controller;
        timer = 0f;
    }

    public void OnReleased()
    {
        isHeld = false;
        waitingCorrection = false;
        holdingController = OVRInput.Controller.None;
    }

    void Update()
    {
        if (!active || !isHeld) return;

        timer += Time.deltaTime;

        if (!waitingCorrection && timer >= shiftInterval / instabilityScale)
        {
            requiredTiltDir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
            waitingCorrection = true;
            timer = 0f;
        }

        if (waitingCorrection)
        {
            transform.localPosition = baseLocalPos + Random.insideUnitSphere * 0.01f * instabilityScale;

            Quaternion controllerRot = OVRInput.GetLocalControllerRotation(holdingController);
            Vector3 controllerUp = controllerRot * Vector3.up;

            float alignment = Vector3.Dot(controllerUp, requiredTiltDir);
            if (alignment < -Mathf.Cos(tiltAngleRequired * Mathf.Deg2Rad))
            {
                waitingCorrection = false;
                timer = 0f;
                transform.localPosition = baseLocalPos;
            }
            else if (timer >= correctionWindow / instabilityScale)
            {
                Drop();
            }
        }
    }

    void Drop()
    {
        waitingCorrection = false;
        isHeld = false;
        transform.SetParent(null);
        var rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
        GetComponent<OVRGrabbable>()?.ForceRelease();
    }

    public void SetDifficulty(float scale) => instabilityScale = scale;
}
