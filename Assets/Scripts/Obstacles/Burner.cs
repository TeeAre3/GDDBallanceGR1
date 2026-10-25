using System.Reflection.Metadata.Ecma335;
using UnityEngine;

public class Burner : MonoBehaviour
{
    [Header("Nagrzewanie metalu")]
    [Tooltip("Ile temperatury (0-1) kula zyskuje na sekundê w p³omienu")]
    [SerializeField] float heatRate = 0.5f;

    [Header("Cykl pracy (opcjonalnie)")]
    [Tooltip("0 = palnik p³onie ca³y czas")]
    [SerializeField] float onTime = 0f;
    [SerializeField] float offTime = 2f;

    [Header("Wygl¹d")]
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
