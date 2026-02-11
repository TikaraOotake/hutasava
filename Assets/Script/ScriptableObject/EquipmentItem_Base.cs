using UnityEngine;
using StructStatus;

[CreateAssetMenu(fileName = "EquipmentItem_Base", menuName = "ScriptableObjects/CreateEquipmentItem_Base")]
public class EquipmentItem_Base : ScriptableObject
{
    [SerializeField] private Sprite ItemSprite;//画像
    [SerializeField] private string ItemName;//名前
    [SerializeField] private string ItemExplanation;//アイテム説明文

    [SerializeField] private int SaleCost = 1;//価格

    [SerializeField] private float Atk;
    [SerializeField] private float HP;
    [SerializeField] private float Speed;
    [SerializeField] private int Level = 1;

    //アイテムの初期化
    public virtual void Init()
    {
        return;
    }
    //アイテムの解放
    public virtual void Remove()
    {
        return;
    }

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
    public int GetLevel()
    {
        return Level;
    }
    public void SetLevel(int _Level)
    {
        Level = _Level;
    }
    public CharacterStatus GetStatus()
    {
        CharacterStatus status;
        status.AttackPoint = Atk;
        status.HealthPoint = HP;
        status.SpeedPoint= Speed;
        return status;
    }
}
