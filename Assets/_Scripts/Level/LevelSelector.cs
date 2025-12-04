using UnityEngine;

public class LevelSelector : MonoBehaviour {
    [Header("Level Info")]
    public int levelID;
    public string levelName;
    public Sprite previewImage;

    [Header("UI Manager")]
    public LevelSelectUIManager uiManager;   // Kéo từ hierarchy vào

    private void OnMouseDown() {
        // Khi click vào object có Collider
        if (uiManager != null) {
            uiManager.ShowLevelInfo(levelID, levelName, previewImage);
        }
        else {
            Debug.LogWarning("LevelSelector: uiManager chưa được gán!");
        }
    }
}
