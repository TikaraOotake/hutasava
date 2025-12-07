using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentItem_Base", menuName = "ScriptableObjects/CreateEquipmentItem_Base")]
public class EquipmentItem_Base :  ScriptableObject
{
    [SerializeField] private string ItemName;
    [SerializeField] private string ItemExplanation;//ƒAƒCƒeƒ€à–¾•¶
}
