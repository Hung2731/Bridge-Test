using UnityEngine;

public class CarTest : MonoBehaviour
{
    public float speed = 4f;
    private Rigidbody rb;

    void Start() {
        rb = GetComponent<Rigidbody>();

        // Đề xuất setup vật lý để tránh xuyên
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        // Khóa trục không cần
        rb.constraints = RigidbodyConstraints.FreezePositionZ |
                         RigidbodyConstraints.FreezeRotationX |
                         RigidbodyConstraints.FreezeRotationZ;
    }

    void FixedUpdate() {
        Vector3 moveDir = transform.right; // hướng xe đang nhìn
        rb.linearVelocity = moveDir * speed + Vector3.up * rb.linearVelocity.y;
    }
}
