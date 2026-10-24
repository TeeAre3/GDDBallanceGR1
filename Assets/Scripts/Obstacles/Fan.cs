using UnityEngine;

public class Fan : MonoBehaviour
{
    [Header("Moc")]
    [Tooltip("Przyspieszenie nadawane kuli w m/s². Żeby kula się wznosiła, musi być większe niż 9.81")]
    [SerializeField] float power = 16f;

    [Tooltip("Włącz, jeśli siła ma słabnąć z odległością od wiatraka")]
    [SerializeField] bool useFalloff = false;
    [Tooltip("Zasięg liczony dla falloff (powinien odpowiadać długości collidera w osi Z)")]
    [SerializeField] float range = 6f;

    [Header("Wygląd (opcjonalnie)")]
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
