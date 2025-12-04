using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class BarMaterialOption {
    public BarMaterialType type;       // Road, Wood, Steel
    public GameObject barPrefab;       // Prefab của vật liệu
    public Button button;              // Button UI
    public Outline outline;            // Viền highlight nút
}
