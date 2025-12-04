using System.Collections.Generic;
using UnityEngine;

public class GameManager_Test : MonoBehaviour {
    public LevelController levelController;
    public static Dictionary<Vector3Int, Point> AllPoints = new Dictionary<Vector3Int, Point>();
    public static List<Bar> AllBars = new List<Bar>();
    

    private void Awake() {
        AllPoints.Clear();
        AllBars.Clear();
        Time.timeScale = 0f;
        //Instantiate(Resources.Load("Level/Level 1"));
    }

    [ContextMenu("Pause")]
    public void Pause() {
        Time.timeScale = 0f;
    }
}
