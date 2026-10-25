using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillZone : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        var ball = other.GetComponentInParent<BallMaterialController>();
        if (ball == null) return;

        PlayerDeath.Die(ball.gameObject);
    }
}
