using UnityEngine;

public class WinCheckPoint : MonoBehaviour {
    [SerializeField] private GameObject winUI;   
    public float stopSpeedTime = 0.3f;           

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Car")) {
            Debug.Log("Win Checkpoint Triggered by Car");
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null) {
                StartCoroutine(SlowDownAndWin(rb));
            }
        }
    }

    private System.Collections.IEnumerator SlowDownAndWin(Rigidbody rb) {
        float elapsed = 0f;
        Vector3 initialVelocity = rb.linearVelocity;

        while (elapsed < stopSpeedTime) {
            elapsed += Time.deltaTime;
            rb.linearVelocity = Vector3.Lerp(initialVelocity, Vector3.zero, elapsed / stopSpeedTime);
            yield return null;
        }

        rb.linearVelocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeAll;

        if (winUI != null)
            winUI.SetActive(true);
    }
}
