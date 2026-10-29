using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "DemoLevel";

    public void StartGame()
    {
        Time.timeScale = 1f;
        GameSession.ResetLives();
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
}