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
}
