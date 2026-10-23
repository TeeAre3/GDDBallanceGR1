using UnityEngine;
using PhysMat = UnityEngine.PhysicsMaterial;

[CreateAssetMenu(menuName = "Element Balance/Ball Material", fileName = "NewBallMaterial")]
public class BallMaterialData : ScriptableObject
{
    public BallMaterialType type;

    [Header("Physics")]
    public float mass = 1f;
    public PhysMat physicsMaterial;
    [Tooltip("How Quickly the ball's spin dies down. Low = slippery feel")]
    public float angularDamping = 0.5f;
    [Tooltip("Air-drag style slowdown of movement. Usually 0.")]
    public float linearDamping = 0f;

    [Header("Handling")]
    [Tooltip("Acceleration from input.")]
    public float torqueStrength = 20f;
    [Tooltip("Cap on spin speed.")]
    public float maxAngularVelocity = 30f;
    [Tooltip("Cap on horizontal speed in m/s.")]
    public float maxLinearSpeed = 10f;


    [Header("Look")]
    public Material visualMaterial;
}
