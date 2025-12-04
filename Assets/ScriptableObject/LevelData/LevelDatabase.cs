using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDatabase", menuName = "BridgeMaster/LevelDatabase")]
public class LevelDatabase : ScriptableObject {
    public List<LevelData> levels = new List<LevelData>();

    public LevelData GetLevelByID(int id) {
        return levels.Find(l => l.levelID == id);
    }
}
