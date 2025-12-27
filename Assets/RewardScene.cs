using UnityEngine;
using System.Collections.Generic;

public class RewardScene :MonoBehaviour
{
    [SerializeField] int HoldSelectIndex;//保留選択中のインデックス

    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private ItemContainer itemContainer;

    //リスト同期用変数
    private int prevA, prevB;

    void Start()
    {
        if (itemContainer != null)
        {
            //入力時のイベントを登録
            itemContainer.SetClickEvent(OnClick_UI);
        }


        if (playerManager == null)
        {
            //Playerマネージャー取得
            playerManager = GameManager.Instance.GetPlayerManager();
        }

        if (itemContainer != null)
        {
            List<UI_ItemSlot_V2> uiList = itemContainer.GetItem_DisplayUI_List();
            //選択可能状態にしたい選択肢を登録
            GameManager.Instance.SetSelectSlot_isSelective(uiList);
        }


        //アイテム生成
        GenerateItem();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnClick_UI(UI_Base _ui)
    {
        if (itemContainer != null)
        {
            List<UI_ItemSlot_V2> uiList = itemContainer.GetItem_DisplayUI_List();
            List<EquipmentItem_Base> itemList = itemContainer.GetItemList();
            for (int i = 0; i < uiList.Count; ++i)
            {
                if (uiList[i] == _ui)
                {
                    //Debug.Log(i + "番スロットが呼ばれました");

                    //残高取得
                    int money = GameManager.Instance.GetMoney();

                    //残高を値段で引く
                    money -= itemList[i].GetSaleCost();

                    bool Result = false;

                    //残高が0以上ならアイテム交換
                    if (money >= 0)
                    {
                        Result = TransferItem_toStorage(itemList[i]);
                    }

                    //交換に成功したら
                    if (Result) 
                    {
                        GameManager.Instance.SetMoney(money);//残高をManagerに代入
                        itemContainer.SetItem(i, null);//該当要素番号のアイテムを削除
                    }
                    
                    
                    return;
                }
            }
        }
    }
    public bool TransferItem_toStorage(EquipmentItem_Base _Itemdata)
    {
        List<EquipmentItem_Base> StorageList = new List<EquipmentItem_Base>();
        if (playerManager != null)
        {
            ItemContainer PlayerItemContainer = playerManager.GetItemContainer();

            if(PlayerItemContainer!=null)
            {
                //プレイヤーマネージャーからストレージの内容を取得
                StorageList = PlayerItemContainer.GetItemStorageList();

                for (int i = 0; i < StorageList.Count; ++i)
                {
                    //既に入っているアイテムを取得
                    EquipmentItem_Base item = StorageList[i];

                    //アイテムが空であれば引き数のアイテムを代入して終了
                    if (item == null)
                    {
                        PlayerItemContainer.SetItem(_Itemdata, i);
                        return true;
                    }
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
        if (itemContainer != null)
        {
            List<EquipmentItem_Base> itemList = itemContainer.GetItemStorageList();
            for (int i = 0; i < itemList.Count; ++i)
            {
                //ランダムなアイテムデータを渡す
                itemContainer.SetItem(i, GameManager.Instance.GetRandCopyItemData());
            }
        }
    }
}
