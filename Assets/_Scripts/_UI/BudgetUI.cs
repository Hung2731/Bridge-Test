using UnityEngine;
using UnityEngine.UI;

public class BudgetUI : MonoBehaviour {
    public Text budgetText;   
    private LevelBudgetManager budgetManager;

    void Start() {
        budgetManager = FindObjectOfType<LevelBudgetManager>();

        // Lắng nghe sự kiện thay đổi ngân sách
        budgetManager.OnBudgetChanged.AddListener(UpdateUI);

        // Cập nhật ngay lần đầu
        UpdateUI(budgetManager.spent, budgetManager.levelBudget);
    }

    void UpdateUI(float spent, float budget) {
        budgetText.text = "$" + spent.ToString("0") + " / " + "$" + budget.ToString("0");
    }
}
