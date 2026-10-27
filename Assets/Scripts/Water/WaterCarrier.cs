using System;
using System.Net.Http.Headers;
using UnityEditor.ShaderGraph;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(BallMaterialController))]
public class WaterCarrier : MonoBehaviour
{
    [Range(0f, 1f)] public float water = 0f;
    public bool IsWet => water > 0.05f;

    [Header("Wet weight & look")]
    [Tooltip("Mass added at full water. (dry mass + this = wet mass)")]
    public float extraMassWhenWet = 4.5f;
    public Color dryTint = new(1f, 1f, 0f);
    public Color wetTint = new(0.4f, 0.4f, 0f);

    public event Action<float> OnWaterChanged;

    private Rigidbody rb;
    private BallMaterialController ball;
    private Renderer rend;
    private MaterialPropertyBlock block;
    private float baseMass;

    private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
    private static readonly int ColorId = Shader.PropertyToID("_Color");

    private bool IsSponge => ball.Current == BallMaterialType.Sponge;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        ball = GetComponent<BallMaterialController>();
        rend = GetComponent<Renderer>();
        block = new MaterialPropertyBlock();

        rb.sleepThreshold = 0f;
    }

    void OnEnable() { ball.OnMaterialChanged += HandleMaterialChanged; }
    void OnDisable() { ball.OnMaterialChanged -= HandleMaterialChanged; }

    private void HandleMaterialChanged(BallMaterialType type)
    {
        if(type == BallMaterialType.Sponge)
        {
            baseMass = rb.mass;
        }
        else
        {
            water = 0f;
            if (rend != null) rend.SetPropertyBlock(null);
        }
        Refresh();
    }

    void OnValidate()
    {
        if (!Application.isPlaying || rb == null || ball == null) return;
        Refresh();
    }

    public float Add(float amount)
    {
        if(!IsSponge || amount <= 0f) return 0f;
        float before = water;
        water = Mathf.Clamp01(water + amount);
        Refresh();
        return water - before;
    }

    public float Remove(float amount)
    {
        if(amount <= 0f) return 0f;
        float before = water;
        water = Mathf.Max(0f, water - amount);
        Refresh();
        return before - water;
    }

    private void Refresh()
    {
        if(IsSponge)
        {
            rb.mass = baseMass + extraMassWhenWet * water;

            if(rend != null)
            {
                Color tint = Color.Lerp(dryTint, wetTint, water);
                block.SetColor(BaseColorId, tint);
                block.SetColor(ColorId, tint);
                rend.SetPropertyBlock(block);
            }
        }
    }
}
