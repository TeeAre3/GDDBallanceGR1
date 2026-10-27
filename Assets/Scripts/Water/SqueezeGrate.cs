using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SqueezeGrate : MonoBehaviour
{
    public WaterBucket bucket;
    [Tooltip("Water squeezed out per second, in loads.")]
    public float squeezePerSecond = 0.4f;

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (bucket == null || bucket.IsFull) return;

        Rigidbody body = other.attachedRigidbody;
        if(body == null) return;

        WaterCarrier carrier = body.GetComponent<WaterCarrier>();
        if (carrier == null || carrier.water <= 0f) return;

        float wanted = Mathf.Min(squeezePerSecond * Time.deltaTime, carrier.water);
        float accepted = bucket.Add(wanted);
        carrier.Remove(accepted);
    }
}
