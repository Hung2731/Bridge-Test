using TMPro;
using UnityEngine;
public class LevelSelector : MonoBehaviour
{
    public TMP_Text levelText;
    [Header("Level Info")]
    public int levelID;
    public string levelName;
    public Sprite previewImage;

    [Header("UI Manager")]
    public LevelSelectUIManager uiManager;   // Kéo từ hierarchy vào

    private void Start()
    {
        SetUpLevelText();
    }

    private void SetUpLevelText() 
    {
        if (levelText != null) {
            levelText.text = levelID.ToString();
            if (levelID > PlayerPrefs.GetInt(Const.PLAYER_MAX_PASSED_LEVEL) + 1) {
                levelText.color = Color.gray;
            }
        }
    }

    private void OnMouseDown()
    {
        // Khi click vào object có Collider
        if (levelID > PlayerPrefs.GetInt(Const.PLAYER_MAX_PASSED_LEVEL) + 1)
            return;
        if (uiManager != null)
        {
            uiManager.ShowLevelInfo(levelID, levelName, previewImage);
        }
    }
}
