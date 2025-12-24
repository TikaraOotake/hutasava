using UnityEngine;
using System.Collections.Generic;

public class ItemContainer : MonoBehaviour
{
    [SerializeField] protected List<EquipmentItem_Base> ItemList;//アイテムを入れておくリスト
    [SerializeField] protected List<UI_ItemSlot_V2> UI_ItemList;//アイテムを表示するUIのリスト
    [SerializeField] protected int ItemIndex;

    public List<EquipmentItem_Base> GetItemList()
    {
        return ItemList;
    }
    public EquipmentItem_Base GetItem(int _Index)
    {
        EquipmentItem_Base item = null;

        if (_Index >= 0 && _Index < ItemList.Count)//配列内チェック
        {
            item = ItemList[_Index];
        }

        return item;
    }
    public virtual bool SetItem(int _Index, EquipmentItem_Base _Item)
    {
        bool Result = false;
        if (_Index >= 0 && _Index < ItemList.Count)//配列内チェック
        {
            ItemList[_Index] = _Item;
            Result = true;
        }
        else
        {
            Debug.Log("入れようとした要素番号が配列外でした");
        }

        //UIの表示更新
        Update_DisplayUI();

        return Result;
    }

    public UI_Base GetItem_DisplayUI(int _Index)
    {
        UI_Base ui = null;

        if (_Index >= 0 && _Index < UI_ItemList.Count)//配列内チェック
        {
            ui = UI_ItemList[_Index];
        }

        return ui;
    }

    //UIの表示を更新する
    protected void Update_DisplayUI()
    {
        //UIの数だけ繰り返し
        for (int i = 0; i < UI_ItemList.Count; ++i)
        {
            if (UI_ItemList[i] != null)
            {
                if (0 <= i && ItemList.Count > i)
                {
                    //アイテムを取得してUIに映したい情報を伝える
                    EquipmentItem_Base item = ItemList[i];
                    if (item != null)
                    {
                        UI_ItemList[i].SetItemShowData(
                            item.GetItemSprite(),
                            item.GetItemName(),
                            item.GetItemExplanation());

                        continue;
                    }
                }

                //アイテムが無効だった場合はなにも表示しない
                UI_ItemList[i].ResetShowData();
            }
        }
    }
}
