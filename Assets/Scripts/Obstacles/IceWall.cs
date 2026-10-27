using UnityEngine;

public class IceWall : MonoBehaviour
{
    [Tooltip("Minimum ball temperature required for the ice to start melting (0-1)")]
    [SerializeField, Range(0f, 1f)] float minTemperature = 0.4f;
    [Tooltip("Seconds of contact with a hot ball needed to melt completely")]
    [SerializeField] float meltTime = 2f;

    Vector3 startScale;
    float bottomY;
    float progress;
    bool melted;

    void Awake()
    {
        startScale = transform.localScale; ;
        bottomY = transform.localPosition.y - startScale.y / 2f;
    }

    void OnCollisionStay(Collision collision)
    {
        if (melted) return;

        var heat = collision.collider.GetComponentInParent<BallHeat>();
        if (heat == null || !heat.IsMetal || heat.temperature < minTemperature) return;

        progress += Time.deltaTime / meltTime;
        ApplyShrink();

        if(progress >= 1f)
        {
            melted = true;
            heat.ClearHeat();
            Destroy(gameObject);
        }
    }

    void ApplyShrink()
    {
        float h = startScale.y * (1f - Mathf.Clamp01(progress));
        var s = startScale; s.y = Mathf.Max(0.001f, h);
        transform.localScale = s;

        var p = transform.localPosition;
        p.y = bottomY + s.y / 2f;
        transform.localPosition = p;
    }
}
