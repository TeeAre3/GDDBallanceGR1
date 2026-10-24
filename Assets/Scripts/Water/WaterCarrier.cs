using UnityEngine;

public class WaterCarrier : MonoBehaviour
{
    [Range(0f, 1f)] public float water = 0f;
    public bool IsWet => water > 0.05f;
}
