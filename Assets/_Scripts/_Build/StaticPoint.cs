using UnityEngine;

public class StaticPoint : Point {
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start() {
        PointId = Vector3Int.RoundToInt(transform.position);
        if (GameManager_Test.AllPoints.ContainsKey(PointId) == false) {
            GameManager_Test.AllPoints.Add(PointId, this);
        }
        pointRigidbody = GetComponent<Rigidbody>();
        pointRigidbody.isKinematic = true;
    }
}
