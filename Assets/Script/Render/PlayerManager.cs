using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //ストレージのアイテム欄
    [SerializeField] List<EquipmentItem_Base> ItemStorageList;
    [SerializeField] List<UI_ItemSlot_V2> UI_ItemStorageList;

    //Player
    [SerializeField] List<GameObject> PlayerList;//プレイヤーを格納するためのリスト

    //リスト同期用変数
    private int prevA, prevB;

    void Start()
    {
        for (int i = 0; i < UI_ItemStorageList.Count; ++i)
        {
            UI_ItemStorageList[i].OnSelected += OnClick_UI;
        }
    }
    private void Update()
    {
        
    }

    private void OnClick_UI(UI_Base ui)
    {
        //どこのUIが呼ばれたか特定する
        for (int i = 0; i < UI_ItemStorageList.Count; ++i)
        {
            if (UI_ItemStorageList[i] == ui)
            {

                return;
            }
        }
    }
    public List<EquipmentItem_Base> GetItemStorageList()
    {
        return ItemStorageList;
    }
    public void SetItem(EquipmentItem_Base _item, int _index)
    {
        if (0 <= _index && ItemStorageList.Count > _index)
        {
            ItemStorageList[_index] = _item;
        }
    }
    public List<UI_ItemSlot_V2> GetUI_ItemStorageList()
    {
        return UI_ItemStorageList;
    }

    private void OnValidate()
    {
        //リストサイズ同期用処理

        // どれが変更されたか判定
        bool aChanged = ItemStorageList.Count != prevA;
        bool bChanged = UI_ItemStorageList.Count != prevB;

        // 変更されたリストのサイズを基準にする
        int targetSize = -1;

        if (aChanged) targetSize = ItemStorageList.Count;
        else if (bChanged) targetSize = UI_ItemStorageList.Count;
        else return; // 何も変わってなければ終了

        // サイズ同期
        SyncSize(ItemStorageList, targetSize);
        SyncSize(UI_ItemStorageList, targetSize);

        // サイズを更新して次回比較に使う
        prevA = ItemStorageList.Count;
        prevB = UI_ItemStorageList.Count;
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
