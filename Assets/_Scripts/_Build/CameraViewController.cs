using UnityEngine;

public class CameraViewController : MonoBehaviour {
    [Header("Plane XY dùng để dựng cầu")]
    public GameObject bridgePlane;

    [Header("Tham số chuyển góc nhìn 3D")]
    public Vector3 offsetPosition = new Vector3(0, 2, -5);
    public Vector3 rotationEuler = new Vector3(20f, 15f, 0f);
    public float transitionDuration = 2f;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private bool is3DView = false;

    void Start() {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    // Hàm gọi khi bạn ấn Play (bắt đầu mô phỏng)
    public void SwitchTo3DView() {
        if (is3DView) return;
        is3DView = true;
        if (bridgePlane != null) bridgePlane.SetActive(false);
        StopAllCoroutines();
        StartCoroutine(MoveCamera(startPosition, offsetPosition, startRotation, Quaternion.Euler(rotationEuler)));
    }

    // Hàm gọi khi bạn restart
    public void ResetView() {
        is3DView = false;
        if (bridgePlane != null) bridgePlane.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(MoveCamera(transform.position, startPosition, transform.rotation, startRotation));
    }

    private System.Collections.IEnumerator MoveCamera(Vector3 fromPos, Vector3 toPos, Quaternion fromRot, Quaternion toRot) {
        float t = 0;
        while (t < transitionDuration) {
            t += Time.deltaTime;
            float progress = Mathf.SmoothStep(0, 1, t / transitionDuration);
            transform.position = Vector3.Lerp(fromPos, toPos, progress);
            transform.rotation = Quaternion.Slerp(fromRot, toRot, progress);
            yield return null;
        }
    }
}
