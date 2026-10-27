using System;
using UnityEngine;

public class WaterBucket : MonoBehaviour
{
    [Tooltip("How many sponge loads the bucket holds. 1 = one trip, 2 = two trips.")]
    public float capacity = 1f;

    public Transform waterVisual;

    [Tooltip("Current amount stored.")]
    [SerializeField] private float fill;
    public float Fill => fill;
    public float Fill01 => capacity > 0f ? Mathf.Clamp01(Fill / capacity) : 0f;
    public bool IsFull => Fill >= capacity - 0.0001f;

    public event Action<float> OnFillChanged;

    private Vector3 fullScale;
    private Vector3 fullPosition;

    void Awake()
    {
        if (waterVisual != null)
        {
            fullScale = waterVisual.localScale;
            fullPosition = waterVisual.localPosition;
        }
        UpdateVisual();
    }

    public float Add(float amount)
    {
        float accepted = Mathf.Clamp(amount, 0f, capacity - Fill);
        if (accepted <= 0f) return 0f;

        fill += accepted;
        UpdateVisual();
        OnFillChanged?.Invoke(Fill01);
        return accepted;
    }

    private void UpdateVisual()
    {
        if (waterVisual == null) return;

        float h = Mathf.Max(0.001f, Fill01);
        waterVisual.localScale = new Vector3(fullScale.x, fullScale.y * h, fullScale.z);
        waterVisual.localPosition = fullPosition - new Vector3(0f, fullScale.y * (1f - h) * 0.5f, 0f);
    }
}