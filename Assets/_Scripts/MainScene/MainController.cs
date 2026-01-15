using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{
    private void Start()
    {
        AudioManager.instance.PlayBGM(BackgroundMusic.MainTheme);
    }

    public void OpenChooseLevelScene()
    {
        SceneManager.LoadScene(Const.SCENE_LEVEL);
    }

    public void QuitGame() {
#if UNITY_EDITOR
        // Stop play mode when testing inside the Editor
        EditorApplication.isPlaying = false;
#else
        // Quit the built application
        Application.Quit();
#endif
    }
}
