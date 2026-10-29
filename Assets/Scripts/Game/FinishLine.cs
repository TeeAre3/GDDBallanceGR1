using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider))]
public class FinishLine : MonoBehaviour
{
    [Header("UI")]
    [Tooltip("Panel shown on completion")]
    public GameObject completePanel;

    [Header("Scenes")]
    public string menuSceneName = "MainMenu";

    public float stopDamping = 3f;

    private bool finished;
    
    void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    void Start()
    {
        if (completePanel != null) completePanel.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (finished) return;

        BallMaterialController ball = other.GetComponentInParent<BallMaterialController>();
        if (ball == null) return;
        finished = true;

        BallController controller = ball.GetComponent<BallController>();
        if (controller != null) controller.enabled = false;

        Rigidbody rb = ball.GetComponent<Rigidbody>();
        if(rb != null)
        {
            rb.linearDamping = stopDamping;
            rb.angularDamping = stopDamping;
        }
        
        if(completePanel != null) completePanel.SetActive(true);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(menuSceneName);
    }

    public void RestartLevel()
    {
        GameSession.ResetLives();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
