using UnityEngine;
using System.Collections.Generic;

public class RewardScene : ItemContainer
{
    [SerializeField] int HoldSelectIndex;//保留選択中のインデックス

    [SerializeField] private PlayerManager playerManager;

    //リスト同期用変数
    private int prevA, prevB;

    void Start()
    {
        for (int i = 0; i < UI_ItemList.Count; ++i)
        {
            UI_ItemList[i].OnSelected += OnClick_UI;
        }

        if (playerManager == null)
        {
            playerManager = GameManager.Instance.GetPlayerManager();
        }

        //選択可能状態にしたい選択肢を登録
        GameManager.Instance.SetSelectSlot_isSelective(UI_ItemList);

        //アイテム生成
        GenerateItem();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnClick_UI(UI_Base ui)
    {
        //どこのUIが呼ばれたか特定する
        for (int i = 0; i < UI_ItemList.Count; ++i)
        {
            if (UI_ItemList[i] == ui)
            {
                //Debug.Log(i + "番スロットが呼ばれました");

                //該当要素番号のアイテムを交換に掛ける
                TransferItem_toStorage(ItemList[i]);
                return;
            }
        }
    }
    public bool TransferItem_toStorage(EquipmentItem_Base _Itemdata)
    {
        List<EquipmentItem_Base> StorageList = new List<EquipmentItem_Base>();
        if (playerManager != null)
        {
            //プレイヤーマネージャーからストレージの内容を取得
            StorageList = playerManager.GetItemStorageList();

            for (int i = 0; i < StorageList.Count; ++i)
            {
                //既に入っているアイテムを取得
                EquipmentItem_Base item = StorageList[i];

                //アイテムが空であれば引き数のアイテムを代入して終了
                if (item == null)
                {
                    playerManager.SetItem(_Itemdata, i);
                    return true;
                }
            }
        }
        return false;
    }

    public void ExchangeItem(int _SelectIndex)
    {
        if (HoldSelectIndex == -1)
        {
            //保留インデックスが未選択状態(-1)なら
            HoldSelectIndex = _SelectIndex;//選択登録
            return;//終了
        }
        else if (_SelectIndex == HoldSelectIndex)
        {
            //選択インデックスと保留インデックスが同じ場合
            HoldSelectIndex = -1;//選択解除
            return;//終了
        }
        else
        {



            return;
        }
    }

    public void GenerateItem()
    {
        for (int i = 0; i < ItemList.Count; ++i)
        {
            //ランダムなアイテムデータを渡す
            ItemList[i] = GameManager.Instance.GetRandCopyItemData();
        }

        Update_DisplayUI();
    }

    
    public List<UI_ItemSlot_V2> GetUI_ItemList()
    {
        return UI_ItemList;
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
