using UnityEngine;

[RequireComponent(typeof(BallMaterialController))]
public class BallFragility : MonoBehaviour
{
    [Tooltip("Minimum impact speed (m/s) along the wall that breaks the glass ball")]
    [SerializeField] float breakSpeed = 3f;

    [Tooltip("Ignore collisions right after the level starts (avoids breaking on spawn)")]
    [SerializeField] float graceTime = 0.5f;

    BallMaterialController mat;
    bool broken;

    void Awake() => mat = GetComponent<BallMaterialController>();

    void OnCollisionEnter(Collision collision)
    {
        if (broken) return;
        if (Time.timeSinceLevelLoad < graceTime) return;
        if (mat.Current != BallMaterialType.Glass) return;

        var breaker = collision.collider.gameObject.GetComponentInParent<GlassBreaker>();
        if (breaker == null) return;

        float impact = Mathf.Abs(Vector3.Dot(collision.relativeVelocity, collision.GetContact(0).normal));

        if (impact >= breakSpeed * breaker.Sensitivity)
        {
            broken = true;
            Shatter();
        }
    }

    void Shatter()
    {
        Debug.Log("Glass ball shattered");
        PlayerDeath.Die(gameObject);
    }
}
