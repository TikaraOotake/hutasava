using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_ItemSlot_V2 : UI_Base
{
    new public event Action<UI_Base> OnSelected;

    [SerializeField] protected Image ItemSpriteImage;//アイテムの画像を表示させるイメージ
    [SerializeField] protected Text ItemNameText;//アイテムの名前を表示させるテキスト
    [SerializeField] protected Text ItemExpText;//アイテムの説明を表示させるテキスト

    [SerializeField] protected bool IsSetedItem;//アイテムセット状態かのフラグ

    public override void Event()
    {
        OnSelected?.Invoke(this);
        return;
    }

    public void SetItemShowData(Sprite _Sprite, string _Name, string _Exp)
    {
        IsSetedItem = true;

        if (ItemSpriteImage != null) ItemSpriteImage.sprite = _Sprite;
        if (ItemExpText != null) ItemExpText.text = _Exp;
        if (ItemNameText != null) ItemNameText.text = _Name;
    }
    public void SetItemShowData(EquipmentItem_Base _item)
    {
        if (_item != null)
        {
            Sprite sprite = _item.GetItemSprite();
            string Name = _item.GetItemName();
            string Exp = _item.GetItemExplanation();
            SetItemShowData(sprite, Name, Exp);
        }
        else
        {
            ResetShowData();
        }
    }
    public void ResetShowData()
    {
        IsSetedItem = false;

        if (ItemSpriteImage != null) ItemSpriteImage.sprite = null;
        if (ItemExpText != null) ItemExpText.text = " ";
        if (ItemNameText != null) ItemNameText.text = " ";
    }
}
