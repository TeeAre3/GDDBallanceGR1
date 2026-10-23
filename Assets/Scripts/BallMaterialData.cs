using UnityEngine;
using PhysMat = UnityEngine.PhysicsMaterial;

[CreateAssetMenu(menuName = "Element Balance/Ball Material", fileName = "NewBallMaterial")]
public class BallMaterialData : ScriptableObject
{
    public BallMaterialType type;

    [Header("Physics")]
    public float mass = 1f;
    public PhysMat physicsMaterial;

    [Header("Handling")]
    public float torqueStrength = 20f;
    public float maxAngularVelocity = 30f;

    [Header("Look")]
    public Material visualMaterial;
}
