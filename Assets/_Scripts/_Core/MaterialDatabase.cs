using UnityEngine;

public class MaterialDatabase : MonoBehaviour {
    public MaterialDefinition wood;
    public MaterialDefinition steel;
    public MaterialDefinition road;

    public MaterialDefinition Get(BarMaterialType type) {
        return type switch {
            BarMaterialType.Wood => wood,
            BarMaterialType.Steel => steel,
            BarMaterialType.Road => road,
            _ => null
        };
    }
}
