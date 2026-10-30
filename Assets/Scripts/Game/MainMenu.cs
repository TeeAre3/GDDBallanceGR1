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
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}