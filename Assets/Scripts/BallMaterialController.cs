using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider), typeof(BallController))]
public class BallMaterialController : MonoBehaviour
{
    [Header("Material assets")]
    public BallMaterialData sponge;
    public BallMaterialData metal;
    public BallMaterialData glass;

    [Header("Start")]
    public BallMaterialType startMaterial = BallMaterialType.Sponge;

    public BallMaterialType Current { get; private set;  }

    public event Action<BallMaterialType> OnMaterialChanged;

    private Rigidbody rb;
    private Collider col;
    private Renderer rend;
    private BallController controller;
    private bool initialised = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        rend = GetComponentInChildren<Renderer>();
        controller = GetComponent<BallController>();
    }

    void Start()
    {
        SetMaterial(startMaterial, force: true);
    }

    public void SetMaterial(BallMaterialType type, bool force = false)
    {
        if (initialised && !force && type == Current) return;

        BallMaterialData data = GetData(type);
        if (data == null)
        {
            Debug.LogError($"BallMaterialController: no data asset assigned for {type}", this);
            return;
        }

        Current = type;
        initialised = true;

        rb.mass = data.mass;
        rb.angularDamping = data.angularDamping;
        rb.linearDamping = data.linearDamping;

        if (data.physicsMaterial != null) col.sharedMaterial = data.physicsMaterial;
        if (data.visualMaterial != null && rend != null) rend.sharedMaterial = data.visualMaterial;
        controller.ApplyMaterial(data.torqueStrength, data.maxAngularVelocity, data.maxLinearSpeed);

        OnMaterialChanged?.Invoke(type);
    }

    private BallMaterialData GetData(BallMaterialType type)
    {
        switch(type)
        {
            case BallMaterialType.Sponge: return sponge;
            case BallMaterialType.Metal: return metal;
            case BallMaterialType.Glass: return glass;
            default: return null;
        }
    }

}
