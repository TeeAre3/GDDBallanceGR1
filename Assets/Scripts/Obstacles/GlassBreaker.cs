using UnityEngine;

public class GlassBreaker : MonoBehaviour
{
    [Tooltip("Optional: multiplier of the break threshold for this wall (1 = default, <1 = more fragile ball")]
    [SerializeField] float sensitivity = 1f;

    public float Sensitivity => sensitivity;
}
