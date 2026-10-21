using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    [Header("Movement")]
    public float torqueStrength = 20f;
    public float maxAngularVelocity = 30f;
    [Tooltip("true = same handling whatever the mass (dry and wet sponge feel alike). " +
             "false = heavy/wet ball accelerates slower.")]
    public bool massIndependent = true;

    private Rigidbody rb;
    private Vector2 input;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
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
        rb.AddTorque(torque * torqueStrength,
                     massIndependent ? ForceMode.Acceleration : ForceMode.Force);
    }
}