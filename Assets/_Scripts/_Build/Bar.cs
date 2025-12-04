using UnityEngine;

public class Bar : MonoBehaviour {
    public MaterialDefinition materialDefinition; 
    public float cost;

    public GameObject originalPrefab;
    public BarMaterialType barMaterialType;

    public float maxLength = 1f;

    public Vector3 StartPosition;
    public MeshRenderer barMeshRenderer;
    public Point startPoint;
    public Point endPoint;
    public BoxCollider barBoxCollider;
    public HingeJoint startJoint;
    public HingeJoint endJoint;
    public void UpdateCreatingBar(Vector3 ToPosition) {
        transform.position = (StartPosition + ToPosition) / 2f;

        Vector3 direction = ToPosition - StartPosition;
        float length = direction.magnitude;

        // Xoay thanh theo hướng của direction, trục vuông góc là Vector3.forward
        transform.rotation = Quaternion.FromToRotation(Vector3.right, direction.normalized);

        transform.localScale = new Vector3(length, 0.1f, 0.1f);

        // barBoxCollider.size = barMeshRenderer.bounds.size;
    }

    public void SetupJoints(Point start, Point end) {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody>();

        // --- Rigidbody setup ---
        rb.mass = 0.5f;
        rb.linearDamping = 0.1f;

        // Freeze ngoài mặt phẳng XY → chỉ cho phép chuyển động, xoay trong mặt phẳng
        rb.constraints = RigidbodyConstraints.FreezePositionZ |
                         RigidbodyConstraints.FreezeRotationX |
                         RigidbodyConstraints.FreezeRotationY;

        Vector3 localStart = new Vector3(-0.5f, 0f, 0f); // trái
        Vector3 localEnd = new Vector3(0.5f, 0f, 0f);    // phải

        // --- HingeJoint cho đầu Start ---
        startJoint = gameObject.AddComponent<HingeJoint>();
        startJoint.connectedBody = start.pointRigidbody;
        startJoint.autoConfigureConnectedAnchor = false;
        startJoint.anchor = transform.InverseTransformPoint(start.transform.position);
        startJoint.connectedAnchor = Vector3.zero;
        startJoint.axis = Vector3.forward; // Quan trọng: chỉ xoay quanh trục Z
        startJoint.useLimits = false;
        startJoint.enableCollision = false;
        startJoint.enablePreprocessing = false;

        // --- HingeJoint cho đầu End ---
        endJoint = gameObject.AddComponent<HingeJoint>();
        endJoint.connectedBody = end.pointRigidbody;
        endJoint.autoConfigureConnectedAnchor = false;
        endJoint.anchor = transform.InverseTransformPoint(end.transform.position); ;
        endJoint.connectedAnchor = Vector3.zero;
        endJoint.axis = Vector3.forward; // Giống trên
        endJoint.useLimits = false;
        endJoint.enableCollision = false;
        endJoint.enablePreprocessing = false;
    }

    public float GetLength() {
        if (startPoint != null && endPoint != null) {
            return Vector3.Distance(startPoint.transform.position, endPoint.transform.position);
        }
        return Mathf.Abs(transform.localScale.x);
    }

    public void SetMaterial(MaterialDefinition def) {
        materialDefinition = def;
        barMaterialType = def.materialType;   // đồng bộ enum

        // giới hạn chiều dài
        maxLength = def.maxLength;
    }

    public void CalculateCost() {
        if (materialDefinition == null || startPoint == null || endPoint == null) {
            cost = 0;
            return;
        }

        float length = Vector3.Distance(startPoint.transform.position, endPoint.transform.position);
        cost = length * materialDefinition.costPerUnit;
    }
}
public enum BarMaterialType {
    Wood,
    Steel,
    Road
}
