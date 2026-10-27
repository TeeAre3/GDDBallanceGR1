using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BucketBridge : MonoBehaviour
{
    public WaterBucket bucket;

    [Header("Raised pose, relative to the lowered pose")]
    public Vector3 raisedPositionOffset = new Vector3(0f, 4f, 0f);
    public Vector3 raisedEulerOffset = Vector3.zero;

    public float moveSpeed = 0.5f;

    private Rigidbody rb;
    private Vector3 loweredPos, raisedPos;
    private Quaternion loweredRot, raisedRot;
    private float progress;

    void Reset()
    {
        var body = GetComponent<Rigidbody>();
        body.isKinematic = true;
    }

    void Awake()
    {
        if (bucket != null && !bucket.gameObject.scene.IsValid())
            Debug.LogWarning("[BucketBridge] The Bucket slot points to a prefab asset, not the bucket " +
                             "in the scene. Drag the PH_Bucket from the Hierarchy instead.", this);

        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        loweredPos = transform.position;
        loweredRot = transform.rotation;
        raisedPos = loweredPos + raisedPositionOffset;
        raisedRot = Quaternion.Euler(raisedEulerOffset) * loweredRot;

        progress = 0f;
        transform.SetPositionAndRotation(raisedPos, raisedRot);
    }

    void FixedUpdate()
    {
        float target = bucket != null ? bucket.Fill01 : 0f;
        progress = Mathf.MoveTowards(progress, target, moveSpeed * Time.fixedDeltaTime);

        rb.MovePosition(Vector3.Lerp(raisedPos, loweredPos, progress));
        rb.MoveRotation(Quaternion.Slerp(raisedRot, loweredRot, progress));
    }
}