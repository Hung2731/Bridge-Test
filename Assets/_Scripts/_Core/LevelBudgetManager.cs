using UnityEngine;

public class LevelBudgetManager : MonoBehaviour {
    public LevelData levelData;

    public float levelBudget;
    public float spent = 0f;

    public UnityEngine.Events.UnityEvent<float, float> OnBudgetChanged;

    private void Start() {
        levelBudget = levelData.budget;   // Lấy ngân sách từ LevelData
        OnBudgetChanged?.Invoke(spent, levelBudget);
    }

    public bool CanAfford(float cost) {
        return spent + cost <= levelBudget;
    }

    public void Spend(float cost) {
        spent += cost;
        OnBudgetChanged?.Invoke(spent, levelBudget);
    }

    public void Refund(float cost) {
        spent -= cost;
        if (spent < 0) spent = 0;
        OnBudgetChanged?.Invoke(spent, levelBudget);
    }
}
