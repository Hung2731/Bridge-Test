using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {
    public LevelDatabase database;   // Danh sách LevelData
    public int levelID = 1;          // Level mặc định
    public Transform levelRoot;      // ⭐ Parent chứa level khi load
    public GameObject winPanel;

    private GameObject levelInstance;

    private void Awake() {
        LoadLevel(PlayerPrefs.GetInt(Const.CURRENT_LEVEL));
    }

    public void LoadLevel(int id) {
        levelID = id;

        LevelData data = database.GetLevelByID(id);
        if (data == null) {
            Debug.LogError($"Không tìm thấy LevelData với ID {id}");
            return;
        }

        // Gắn ngân sách cho LevelBudgetManager
        LevelBudgetManager lbm = FindObjectOfType<LevelBudgetManager>();
        if (lbm != null) {
            lbm.levelData = data;
            lbm.levelBudget = data.budget;
            lbm.OnBudgetChanged?.Invoke(lbm.spent, lbm.levelBudget);
        }

        // ⭐ Xóa level cũ trong LevelRoot
        if (levelInstance != null)
            Destroy(levelInstance);

        // ⭐ Nếu Root có object con cũ → xóa hết
        if (levelRoot != null) {
            for (int i = levelRoot.childCount - 1; i >= 0; i--)
                Destroy(levelRoot.GetChild(i).gameObject);
        }

        // ⭐ Load prefab level và đặt vào LevelRoot
        if (data.levelPrefab != null) {
            if (levelRoot != null)
                levelInstance = Instantiate(data.levelPrefab, levelRoot);
            else
                levelInstance = Instantiate(data.levelPrefab);

            Debug.Log($" Loaded Level {id} into LevelRoot");
        }
        else {
            Debug.LogWarning($" LevelData {id} KHÔNG CÓ prefab.");
        }
    }
    public void LoadNextLevel() {
        winPanel.SetActive(false);

        int nextLevelID = levelID + 1;

        LevelData nextLevel = database.GetLevelByID(nextLevelID);
        if (nextLevel == null) {
            Debug.Log("🎉 Đã hoàn thành tất cả level!");
            return;
        }
        PlayerPrefs.SetInt(Const.CURRENT_LEVEL, nextLevelID);

        SceneManager.LoadScene(Const.SCENE_GAMEPLAY);
        LoadLevel(nextLevelID);
    }

    public void LoadSceneLevel() {
        SceneManager.LoadScene(Const.SCENE_LEVEL);
    }
}
