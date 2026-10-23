using UnityEngine;

[RequireComponent (typeof(Collider))]
public class MaterialStation : MonoBehaviour
{
    [Tooltip("Material the ball turns into when it touches this station.")]
    public BallMaterialType gives = BallMaterialType.Metal;

    void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        BallMaterialController ball = other.GetComponentInParent<BallMaterialController>();
        if(ball != null) ball.SetMaterial(gives);
    }
}
