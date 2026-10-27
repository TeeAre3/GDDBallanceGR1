using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WaterPool : MonoBehaviour
{
    [Tooltip("Water absorbed per second. 0.5 = 2s to fill (1 = full sponge)")]
    public float absorbPerSecond = 0.5f;

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerStay(Collider other)
    {
        Rigidbody body = other.attachedRigidbody;
        if (body == null) return;

        WaterCarrier carrier = body.GetComponent<WaterCarrier>();
        if(carrier == null) return;

        carrier.Add(absorbPerSecond * Time.deltaTime);
    }
}
