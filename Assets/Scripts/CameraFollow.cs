using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Top-Down View")]
    public Vector3 offset = new Vector3(0f, 12f, -6f);
    [Tooltip("Roughly how long the camera takes to catch up (seconds).")]
    public float smoothTime = 0.15f;

    private Vector3 velocity;

    void Start()
    {
        if (target != null)
            transform.position = target.position + offset;
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desired = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, desired, ref velocity, smoothTime);

        transform.rotation = Quaternion.LookRotation(-offset);
    }
}