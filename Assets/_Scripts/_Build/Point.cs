using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Point : MonoBehaviour {
    public bool Runtime = true;
    public Rigidbody pointRigidbody;
    public Vector3Int PointId;
    public List<Bar> ConnectedBars = new List<Bar>();
    public List<Point> ConnectedPoints = new List<Point>();
    public Point clonePoint;

    protected virtual void Start() {
        PointId = Vector3Int.RoundToInt(transform.position);
        //pointRigidbody = GetComponent<Rigidbody>();
        //pointRigidbody.isKinematic = true;
    }

    private void Update() {
        if (!Runtime && transform.hasChanged) {
            transform.position = new Vector3(
                Mathf.Round(transform.position.x),
                Mathf.Round(transform.position.y),
                0f
            );
            transform.hasChanged = false;
        }
    }
}
