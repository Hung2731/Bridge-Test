using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public float speed = 4f;

    private Rigidbody rb;
    private float fixedZ;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        // Giữ nguyên Z để xe không bay khỏi mặt XY
        Vector3 pos = rb.position;
        pos.z = fixedZ;
        rb.position = pos;

        // Di chuyển theo trục X
        Vector3 velocity = new Vector3(speed, rb.linearVelocity.y, 0f);
        rb.linearVelocity = velocity;
    }
}
