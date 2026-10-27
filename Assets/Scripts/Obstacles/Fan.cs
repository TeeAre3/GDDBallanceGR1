using UnityEngine;

public class Fan : MonoBehaviour
{
    [Header("Power")]
    [Tooltip("Acceleration applied to the ball in m/s². To lift the ball it must be greater than 9.81")]
    [SerializeField] float power = 16f;

    [Tooltip("Enable if the force should weaken with distance from the fan")]
    [SerializeField] bool useFalloff = false;
    [Tooltip("Range used for falloff (should match the collider length along the Z axis)")]
    [SerializeField] float range = 6f;

    [Header("Visuals (optional)")]
    [SerializeField] Transform blades;
    [SerializeField] Vector3 spinAxis = Vector3.forward;
    [SerializeField] float spinSpeed = 720f;
    
    void Update()
    {
        if(blades != null)
            blades.Rotate(spinAxis, spinSpeed * Time.deltaTime, Space.Self);
    }

    void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb == null) return;

        var material = rb.GetComponent<BallMaterialController>();
        if (material == null || material.Current != BallMaterialType.Sponge) return;

        var carrier = rb.GetComponent<WaterCarrier>();
        if (carrier != null && carrier.IsWet) return;

        Vector3 dir = transform.forward;
        float strength = power;

        if (useFalloff)
        {
            float dist = Vector3.Dot(rb.position - transform.position, dir);
            strength *= 1f - Mathf.Clamp01(dist / range);
        }

        rb.AddForce(dir * strength, ForceMode.Acceleration);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, transform.forward * range);
    }
}
