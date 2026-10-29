using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    [Header("Movement")]
    public float torqueStrength = 20f;
    public float maxAngularVelocity = 30f;
    public float maxLinearSpeed = 10f;
    [Tooltip("true = same handling whatever the mass. false = heavy/wet ball accelerates slower.")]
    public bool massIndependent = true;

    [Header("Modifiers (set by other systems)")]
    public float torqueMultiplier = 1f;
    public float speedMultiplier= 1f;

    private Rigidbody rb;
    private Vector2 input;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = maxAngularVelocity;
    }

    public void ApplyMaterial(float newTorque, float newMaxAngularVelocity, float newMaxLinearSpeed)
    {
        torqueStrength = newTorque;
        maxAngularVelocity = newMaxAngularVelocity;
        maxLinearSpeed = newMaxLinearSpeed;
        if (rb == null) rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = maxAngularVelocity;
    }

    void Update()
    {
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        input = Vector2.ClampMagnitude(input, 1f);
    }

    void FixedUpdate()
    {
        if (rb.isKinematic) return; 

        Vector3 torque = new Vector3(input.y, 0f, -input.x);
        rb.AddTorque(torque * torqueStrength * torqueMultiplier,
                     massIndependent ? ForceMode.Acceleration : ForceMode.Force);

        ClampHorizontalSpeed();
    }

    void ClampHorizontalSpeed()
    {
        Vector3 v = rb.linearVelocity;
        Vector3 flat = new Vector3(v.x, 0f, v.z);
        float limit = maxLinearSpeed * speedMultiplier;
        if (flat.magnitude <= limit) return;

        flat = flat.normalized * limit;
        Vector3 clamped = new Vector3(flat.x, v.y, flat.z);
        rb.linearVelocity = clamped;
    }
}