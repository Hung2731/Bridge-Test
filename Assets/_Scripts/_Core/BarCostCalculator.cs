using UnityEngine;

public static class BarCostCalculator {
    public static float CalculateCost(Point a, Point b, MaterialDefinition material) {
        float length = Vector3.Distance(a.transform.position, b.transform.position);
        return length * material.costPerUnit;
    }
}
