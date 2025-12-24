using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : ItemContainer
{
    //Player
    [SerializeField] List<GameObject> PlayerList;//プレイヤーを格納するためのリスト

    //リスト同期用変数
    private int prevA, prevB;

    void Start()
    {
        for (int i = 0; i < UI_ItemList.Count; ++i)
        {
            UI_ItemList[i].OnSelected += OnClick_UI;
        }

        //選択可能状態にしたい選択肢を登録
        GameManager.Instance.SetSelectSlot_isSelective(UI_ItemList);
    }
    private void Update()
    {
        
    }

    private void OnClick_UI(UI_Base ui)
    {
        //どこのUIが呼ばれたか特定する
        for (int i = 0; i < UI_ItemList.Count; ++i)
        {
            if (UI_ItemList[i] == ui)
            {
                GameManager.Instance.SetSelectSlot(this, i);
                return;
            }
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
    public override bool SetItem(int _Index, EquipmentItem_Base _Item)
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

        //UI更新
        Update_DisplayUI();

        return Result;
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
}
