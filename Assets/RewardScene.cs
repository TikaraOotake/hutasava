using UnityEngine;
using System.Collections.Generic;

public class RewardScene : MonoBehaviour
{
    [SerializeField] private List<EquipmentItem_Base> RewardItemList;//報酬のアイテムのリスト
    [SerializeField] private List<UI_ItemSlot_V2> UI_RewardItemList;//報酬のアイテムUIのリスト(インスペクターで紐づけ設定)

    [SerializeField] int HoldSelectIndex;//保留選択中のインデックス

    [SerializeField] private PlayerManager playerManager;

    //リスト同期用変数
    private int prevA, prevB;

    void Start()
    {
        for (int i = 0; i < UI_RewardItemList.Count; ++i)
        {
            UI_RewardItemList[i].OnSelected += OnClick_UI;
        }

        if (playerManager == null)
        {
            playerManager = GameManager.Instance.GetPlayerManager();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnClick_UI(UI_Base ui)
    {
        //どこのUIが呼ばれたか特定する
        for (int i = 0; i < UI_RewardItemList.Count; ++i)
        {
            if (UI_RewardItemList[i] == ui)
            {
                //該当要素番号のアイテムを交換に掛ける
                TransferItem_toStorage(RewardItemList[i]);
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
        for (int i = 0; i < RewardItemList.Count; ++i)
        {
            //ランダムなアイテムデータを渡す
            RewardItemList[i] = GameManager.Instance.GetRandCopyItemData();
        }
    }
    public List<UI_ItemSlot_V2> GetUI_RewardItemList()
    {
        return UI_RewardItemList;
    }

    private void OnValidate()
    {
        //リストサイズ同期用処理

        // どれが変更されたか判定
        bool aChanged = RewardItemList.Count != prevA;
        bool bChanged = UI_RewardItemList.Count != prevB;

        // 変更されたリストのサイズを基準にする
        int targetSize = -1;

        if (aChanged) targetSize = RewardItemList.Count;
        else if (bChanged) targetSize = UI_RewardItemList.Count;
        else return; // 何も変わってなければ終了

        // サイズ同期
        SyncSize(RewardItemList, targetSize);
        SyncSize(UI_RewardItemList, targetSize);

        // サイズを更新して次回比較に使う
        prevA = RewardItemList.Count;
        prevB = UI_RewardItemList.Count;
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
