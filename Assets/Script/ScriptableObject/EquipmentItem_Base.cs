using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentItem_Base", menuName = "ScriptableObjects/CreateEquipmentItem_Base")]
public class EquipmentItem_Base : ScriptableObject
{
    [SerializeField] private Sprite ItemSprite;//画像
    [SerializeField] private string ItemName;//名前
    [SerializeField] private string ItemExplanation;//アイテム説明文

    [SerializeField] private int SaleCost = 1;//売値

    public Sprite GetItemSprite()
    {
        return ItemSprite;
    }
    public string GetItemName()
    {
        return ItemName;
    }
    public string GetItemExplanation()
    {
        return ItemExplanation;
    }

    public int GetSaleCost()
    {
        return SaleCost;
    }
}
