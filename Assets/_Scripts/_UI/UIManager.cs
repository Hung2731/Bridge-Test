using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    [Header("Material Options")]
    public List<BarMaterialOption> materials = new List<BarMaterialOption>();

    [Header("References")]
    public BarCreation barCreation;
    public CameraViewController cameraViewController;

    private void Start() {
        // Chọn mặc định
        SelectMaterial(BarMaterialType.Road);
    }

    public void Play() {
        Time.timeScale = 1f;

        if (cameraViewController != null)
            cameraViewController.SwitchTo3DView();

        barCreation.CreateBridgeSideAndRoad();
    }

    public void Restart() {
        SceneManager.LoadScene("Test");
    }

    public void SelectMaterial(BarMaterialType type) {
        foreach (var m in materials) {
            bool isSelected = m.type == type;

            // Bật/tắt outline nút
            if (m.outline != null)
                m.outline.enabled = isSelected;

            // Gán prefab đúng vật liệu
            if (isSelected)
                barCreation.BarToInstantiate = m.barPrefab;
        }

        barCreation.currentMaterialType = type;
    }

    public void SelectRoad() {
        SelectMaterial(BarMaterialType.Road);
    }

    public void SelectWood() {
        SelectMaterial(BarMaterialType.Wood);
    }

    public void SelectSteel() {
        SelectMaterial(BarMaterialType.Steel);
    }
}
