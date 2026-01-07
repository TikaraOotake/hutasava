using UnityEngine;
using System.Collections.Generic;
using System;

public class ItemContainer : MonoBehaviour
{
    [SerializeField] protected List<EquipmentItem_Base> ItemList = new List<EquipmentItem_Base>();//アイテムを入れておくリスト
    [SerializeField] protected List<UI_ItemSlot_V2> UI_ItemList = new List<UI_ItemSlot_V2>();//アイテムを表示するUIのリスト
    [SerializeField] protected int ItemIndex;

    //リスト同期用変数
    private int prevA, prevB;

    private void Start()
    {
        //UIの表示更新
        Update_DisplayUI();
    }

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
    public bool SetItem(int _Index, EquipmentItem_Base _Item)
    {
        //初期化
        if (_Item != null) _Item.Init();

        bool Result = false;
        if (_Index >= 0 && _Index < ItemList.Count)//配列内チェック
        {
            //入れ替え先のアイテムを初期化
            if (ItemList[_Index] != null) ItemList[_Index].Init();

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
    public List<UI_ItemSlot_V2> GetItem_DisplayUI_List()
    {
        return UI_ItemList;
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
                        UI_ItemList[i].SetItemShowData(item);

                        continue;
                    }
                }

                //アイテムが無効だった場合はなにも表示しない
                UI_ItemList[i].ResetShowData();
            }
        }
    }

    public void SetClickEvent(Action<UI_Base> _action)
    {
        for (int i = 0; i < UI_ItemList.Count; ++i)
        {
            UI_ItemList[i].OnSelected_UI_Base += _action;
        }
    }

    public void OnClick_UI(UI_Base ui)
    {
        //どこのUIが呼ばれたか特定する
        for (int i = 0; i < UI_ItemList.Count; ++i)
        {
            if (UI_ItemList[i] == ui)
            {
                GameManager.Instance.TradeItem(this, i);
                return;
            }
        }
    }

    //アイテムを交換する
    public void TradeItem(UI_Base ui)
    {
        //どこのUIが呼ばれたか特定する
        for (int i = 0; i < UI_ItemList.Count; ++i)
        {
            if (UI_ItemList[i] == ui)
            {
                GameManager.Instance.TradeItem(this, i);
                return;
            }
        }
    }
    //アイテムを購入する
    public void BuyItem(UI_Base ui)
    {
        //どこのUIが呼ばれたか特定する
        for (int i = 0; i < UI_ItemList.Count; ++i)
        {
            if (UI_ItemList[i] == ui)
            {
                //該当要素番号のアイテムを交換に掛ける
                //TransferItem_toStorage(ItemList[i]);
            }
        }
    }

    private void OnValidate()
    {
        //リストサイズ同期用処理

        // どれが変更されたか判定
        bool aChanged = ItemList.Count != prevA;
        bool bChanged = UI_ItemList.Count != prevB;

        // 変更されたリストのサイズを基準にする
        int targetSize = -1;

        if (aChanged) targetSize = ItemList.Count;
        else if (bChanged) targetSize = UI_ItemList.Count;
        else return; // 何も変わってなければ終了

        // サイズ同期
        SyncSize(ItemList, targetSize);
        SyncSize(UI_ItemList, targetSize);

        // サイズを更新して次回比較に使う
        prevA = ItemList.Count;
        prevB = UI_ItemList.Count;
    }
    private void SyncSize<T>(List<T> list, int size)
    {
        if (list.Count == size) return;

        if (list.Count < size)
        {
            while (list.Count < size)
                list.Add(default);
        }
        else
        {
            list.RemoveRange(size, list.Count - size);
        }
    }

    public List<EquipmentItem_Base> GetItemStorageList()
    {
        return ItemList;
    }
    public void SetItem(EquipmentItem_Base _item, int _index)
    {
        if (0 <= _index && ItemList.Count > _index)
        {
            ItemList[_index] = _item;

            //UI更新
            Update_DisplayUI();
        }
    }
    public List<UI_ItemSlot_V2> GetUI_ItemList()
    {
        return UI_ItemList;
    }
}
