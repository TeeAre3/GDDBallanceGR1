using UnityEngine;

[RequireComponent(typeof(BallMaterialController))]
public class BallHeat : MonoBehaviour
{
    [Range(0f, 1f)] public float temperature = 0f;

    [Header("Cooling")]
    [Tooltip("How much temperature (0-1) is lost per second. 0.1 = 10 s from full glow to cold")]
    [SerializeField] float coolRate = 0.1f;

    [Header("Visuals")]
    [SerializeField] Color hotColor = new Color(1f, 0.3f, 0.05f);
    [SerializeField] float emissionIntensity = 2f;

    BallMaterialController mat;
    Renderer rend;
    MaterialPropertyBlock block;
    bool tinted;

    static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
    static readonly int ColorId = Shader.PropertyToID("_Color");
    static readonly int EmissionId = Shader.PropertyToID("_EmissionColor");

    public bool IsMetal => mat.Current == BallMaterialType.Metal;

    void Awake()
    {
        mat = GetComponent<BallMaterialController>();
        rend = GetComponent<Renderer>();
        block = new MaterialPropertyBlock();
    }

    public void AddHeat(float amount)
    {
        if (!IsMetal) return;
        temperature = Mathf.Clamp01(temperature + amount);
    }

    public void ClearHeat() => temperature = 0f;

    void Update()
    {
        if (!IsMetal) temperature = 0f;
        else temperature = Mathf.Max(0f, temperature - coolRate * Time.deltaTime);

        UpdateTint();
    }

    void UpdateTint()
    {
        if (rend == null) return;

        if (temperature <= 0.001f)
        {
            if(tinted) { rend.SetPropertyBlock(null); tinted = false; }
            return;
        }

        Material m = rend.sharedMaterial;
        Color baseCol = Color.white;
        if(m != null)
        {
            if (m.HasProperty(BaseColorId)) baseCol = m.GetColor(BaseColorId);
            else if (m.HasProperty(ColorId)) baseCol = m.GetColor(ColorId);
        }

        Color col = Color.Lerp(baseCol, hotColor, temperature);
        block.SetColor(BaseColorId, col);
        block.SetColor(ColorId, col);
        block.SetColor(EmissionId, hotColor * (temperature * emissionIntensity));
        rend.SetPropertyBlock(block);
        tinted = true;
    }
}
