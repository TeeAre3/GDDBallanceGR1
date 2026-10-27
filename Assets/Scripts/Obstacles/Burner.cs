using System.Reflection.Metadata.Ecma335;
using UnityEngine;

public class Burner : MonoBehaviour
{
    [Header("Metal heating")]
    [Tooltip("How much temperature (0-1) the ball gains per second inside the flame")]
    [SerializeField] float heatRate = 0.5f;

    [Header("Duty cycle (optional)")]
    [Tooltip("0 = the burner stays on all the time")]
    [SerializeField] float onTime = 0f;
    [SerializeField] float offTime = 2f;

    [Header("Visuals")]
    [SerializeField] ParticleSystem flame;

    Collider zone;
    float timer;
    bool active = true;

    void Awake() => zone = GetComponent<Collider>();


    void Update()
    {
        if (onTime <= 0f) return;

        timer += Time.deltaTime;
        float limit = active ? onTime : offTime;
        if(timer >= limit)
        {
            timer = 0f;
            SetActive(!active);
        }
    }

    void SetActive(bool value)
    {
        active = value;
        zone.enabled = value;
        if (flame == null) return;
        if (value) flame.Play();
        else flame.Stop();
    }

    void OnTriggerStay(Collider other)
    {
        var mat = other.GetComponentInParent<BallMaterialController>();
        if (mat == null) return;

        switch (mat.Current)
        {
            case BallMaterialType.Metal:
                var heat = mat.GetComponent<BallHeat>();
                if (heat != null)
                    heat.AddHeat(heatRate * Time.deltaTime);
                break;

            case BallMaterialType.Sponge:
                PlayerDeath.Die(mat.gameObject);
                break;
            default:
                break;
        }
    }
}
