using UnityEngine;

[CreateAssetMenu(fileName = "Material", menuName = "BridgeMaster/MaterialDefinition")]
public class MaterialDefinition : ScriptableObject {
    public BarMaterialType materialType;
    public float costPerUnit = 10f;   // giá mỗi mét
    public float maxLength = 5f;      // độ dài tối đa
}
