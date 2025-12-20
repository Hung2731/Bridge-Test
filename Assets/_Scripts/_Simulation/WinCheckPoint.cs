using UnityEngine;

public class WinCheckPoint : MonoBehaviour {
    public float stopSpeedTime = 0.3f;           

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Car")) {
            WinLevel();
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

        UIManager.Instance.ShowWinUI();
    }

    private void WinLevel() {
        int currentLevel = PlayerPrefs.GetInt(Const.CURRENT_LEVEL);
        if (currentLevel > PlayerPrefs.GetInt(Const.PLAYER_MAX_PASSED_LEVEL))
        {
            PlayerPrefs.SetInt(Const.PLAYER_MAX_PASSED_LEVEL, currentLevel);
        }
    }

}
