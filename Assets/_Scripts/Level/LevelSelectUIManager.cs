using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class LevelSelectUIManager : MonoBehaviour {
    [Header("UI Elements")]
    public GameObject panelRoot;        // Root panel (ẩn/hiện)
    public Text levelNameText;          // Text hiển thị tên level
    public Image previewImageUI;        // Image để show preview

    private int currentLevelID = -1;

    private void Start() {
        if (panelRoot != null)
            panelRoot.SetActive(false);
    }

    public void ShowLevelInfo(int levelID, string levelName, Sprite preview) {
        currentLevelID = levelID;

        levelNameText.text = levelName;

        if (preview != null) {
            previewImageUI.sprite = preview;
            previewImageUI.enabled = true;
        }
        else {
            previewImageUI.enabled = false;
        }

        panelRoot.GetComponent<CanvasGroup>().alpha = 0f;
        panelRoot.SetActive(true);
        panelRoot.GetComponent<CanvasGroup>().DOFade(1f, 0.3f);
    }

    public void ClosePanel() {
        panelRoot.SetActive(false);
    }

    // Nút Play sẽ load Scene "Test", lưu LevelID vào PlayerPrefs
    public void OnPlayButtonPressed() {
        PlayLevel();
    }

    public void PlayLevel() {
        if (currentLevelID < 0) {
            Debug.LogError("Không có level nào được chọn!");
            return;
        }

        // Giả sử scene của bạn là Level_1, Level_2, Level_3…
        string sceneName = Const.SCENE_GAMEPLAY;
        PlayerPrefs.SetInt(Const.CURRENT_LEVEL, currentLevelID);
        SceneManager.LoadScene(sceneName);
    }

    // Viết hàm BackToMainMenu để quay lại scene MainMenu
    public void BackToMainMenu() {
        SceneManager.LoadScene(Const.SCENE_MAIN);
    }
}
