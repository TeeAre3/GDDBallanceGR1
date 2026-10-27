using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class PlayerDeath
{
    static Restarter active;

    public static void Die(GameObject ball, float delay = 0.5f)
    {
        if (active != null) return;

        ball.SetActive(false);

        var go = new GameObject("Restarter");
        active = go.AddComponent<Restarter>();
        active.StartCoroutine(active.Run(delay));
    }

    class Restarter : MonoBehaviour
    {
        public IEnumerator Run(float delay)
        {
            GameSession.Lives--;
            Debug.Log($"Lives left: {GameSession.Lives}");

            yield return new WaitForSeconds(delay);

            if (GameSession.Lives <= 0)
                GameSession.ResetLives();

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
