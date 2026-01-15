using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "BridgeMaster/LevelData")]
public class LevelData : ScriptableObject {
    public int levelID;
    public float budget;
    public GameObject levelPrefab;
    public Vector3 buildPlaneScale;

    // Sau này bạn có thể thêm nhiều thứ nữa:
}
