using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "BridgeMaster/LevelData")]
public class LevelData : ScriptableObject {
    public int levelID;
    public float budget;
    public GameObject levelPrefab;
    public Vector3 buildPlaneScale;
}
