using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class RewardScene :MonoBehaviour
{
    [SerializeField] int HoldSelectIndex;//保留選択中のインデックス

    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private ItemContainer itemContainer;

    [SerializeField] private UI_Base UI_BuySlot;
    [SerializeField] private UI_Base UI_CloseEventSlot;//報酬シーンUIを閉じるボタン

    [SerializeField] private UI_SelectWindow SelectAttachmentWindow;//装備者を決めるウィンドウ

    [SerializeField] private Accessory waitAcce;//装備待機アクセ

    //リスト同期用変数
    private int prevA, prevB;

    void Start()
    {
        //入力時のイベントを登録
        if (itemContainer != null) itemContainer.SetClickEvent(OnClick_UI);
        if (UI_BuySlot != null) UI_BuySlot.OnSelected += OnClick_UI_SaleItem;
        if (UI_CloseEventSlot != null) UI_CloseEventSlot.OnSelected += OnClick_UI_CloseEvent;

        if (SelectAttachmentWindow != null)
        {
            //入力時のイベントを登録
            SelectAttachmentWindow.SetAction(0, OnClick_UI_AttachAccessory, 1);
            SelectAttachmentWindow.SetAction(1, OnClick_UI_AttachAccessory, 2);

            SelectAttachmentWindow.SetActiveWindow(false);//非表示
        }


        //Playerマネージャー取得
        if (playerManager == null) playerManager = GameManager.Instance.GetPlayerManager();


        //報酬シーンを開く
        //OpenRewardScene();

        //報酬シーンを閉じる
        CloseRewardScene();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            //CloseRewardScene();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            GenerateItem();
        }
    }
    private void TestFunction(int i)
    {
        Debug.Log("テスト　引数：" + i);
    }

    private void OnClick_UI(UI_Base _ui)
    {
        //コンテナの有効性チェック
        if (itemContainer != null)
        {
            //アイテムとそれに付随するUIのリストを取得
            List<UI_ItemSlot_V2> uiList = itemContainer.GetItem_DisplayUI_List();
            List<EquipmentItem_Base> itemList = itemContainer.GetItemList();

            //リストの要素を1つずつチェック
            for (int i = 0; i < uiList.Count; ++i)
            {
                //要素の有効性チェック
                if (uiList[i] == _ui && itemList[i] != null)
                {
                    //Debug.Log(i + "番スロットが呼ばれました");

                    //残高取得
                    int money = GameManager.Instance.GetMoney();

                    //残高を値段で引く
                    money -= itemList[i].GetSaleCost();

                    bool Result = false;

                   

                    //残高が0以上ならアイテム移す
                    if (money >= 0)
                    {
                        //アイテムの種類を確認
                        if (itemList[i] is Accessory)
                        {
                            //アイテムがアクセサリーであれば次にどちらに装備するか決める

                            //選択ウィンドウの有効化
                            SelectAttachmentWindow.SetActiveWindow(true);

                            //装備したいアクセを待機欄に設定
                            waitAcce = (Accessory)itemList[i];

                            Result = true;
                        }
                        else
                        {
                            //アイテムを移す
                            Result = TransferItem_toStorage(itemList[i]);
                        }
                    }

                    //交換に成功したら
                    if (Result) 
                    {
                        GameManager.Instance.SetMoney(money);//残高をManagerに代入
                        itemContainer.SetItem(i, null);//コンテナの該当要素番号のアイテムを削除
                    }

                    return;
                }
            }
        }
    }
    private void OnClick_UI_SaleItem(int i)
    {
        GameManager.Instance.SaleItem();
    }
    private void OnClick_UI_CloseEvent(int i)
    {
        //報酬シーンUIを閉じる
        CloseRewardScene();

        //ゲーム再開
        GameManager.Instance.Event_PlayGame();
    }
    //アクセサリ装備
    private void OnClick_UI_AttachAccessory(int id)
    {
        if (playerManager != null && waitAcce != null)//有効性チェック
        {
            playerManager.SetAccessory_Player(id, waitAcce);//セット
            waitAcce = null;//待機アクセを空に
        }

        //選択ウィンドウを非有効化
        SelectAttachmentWindow.SetActiveWindow(false);

        //元の報酬シーンのUIを再び選択可能に設定
        SetSelectUI_IsActive();

        Debug.Log("アクセを装備しました");
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
                //アイテムが残っていたら削除する
                EquipmentItem_Base item = itemContainer.GetItem(i);
                if (item != null)
                {
                    Destroy(item);
                    item = null;
                }

                //ランダムなアイテムデータを渡す
                itemContainer.SetItem(i, GameManager.Instance.GetRandCopyItemData());
            }
        }
    }

    //報酬シーンのUIを選べる状態にする
    public void SetSelectUI_IsActive()
    {
        ////選択可能状態にしたいものをリストにまとめる
        List<UI_Base> uiList = new List<UI_Base>();

        if (itemContainer != null)
        {
            List<UI_ItemSlot_V2> tempList = itemContainer.GetItem_DisplayUI_List();
            for (int i = 0; i < tempList.Count; ++i)
            {
                uiList.Add(tempList[i]);//リストに追加
            }
        }

        //売却UIを追加
        uiList.Add(UI_BuySlot);
        uiList.Add(UI_CloseEventSlot);

        if (playerManager != null)
        {
            //プレイヤーストレージのUIを追加
            ItemContainer container = playerManager.GetItemContainer();
            if (container != null)
            {
                List<UI_ItemSlot_V2> tempList = container.GetItem_DisplayUI_List();
                for (int i = 0; i < tempList.Count; ++i)
                {
                    uiList.Add(tempList[i]);//リストに追加
                }
            }
        }

        //PlayerのコンテナのUIを追加
        GameObject Player1 = GameManager.Instance.GetPlayer(1);
        GameObject Player2 = GameManager.Instance.GetPlayer(2);
        PlayerController_3d Player1comp = null;
        PlayerController_3d Player2comp = null;
        if (Player1 != null) Player1comp = Player1.GetComponent<PlayerController_3d>();
        if (Player2 != null) Player2comp = Player2.GetComponent<PlayerController_3d>();
        if (Player1comp != null)
        {
            ItemContainer itemContainer = Player1comp.GetItemContainer();
            if (itemContainer != null)
            {
                List<UI_ItemSlot_V2> tempList = itemContainer.GetItem_DisplayUI_List();
                for (int i = 0; i < tempList.Count; ++i)
                {
                    uiList.Add(tempList[i]);
                }
            }
        }
        if (Player2comp != null)
        {
            ItemContainer itemContainer = Player2comp.GetItemContainer();
            if (itemContainer != null)
            {
                List<UI_ItemSlot_V2> tempList = itemContainer.GetItem_DisplayUI_List();
                for (int i = 0; i < tempList.Count; ++i)
                {
                    uiList.Add(tempList[i]);
                }
            }
        }

        //登録
        GameManager.Instance.SetSelectSlot_isSelective(uiList);

        //UIを表示にする
        for (int i = 0; i < uiList.Count; ++i)
        {
            if (uiList != null)
            {
                uiList[i].gameObject.SetActive(true);
            }
        }
        if (playerManager != null)
        {
            //プレイヤーステータスのUIを表示
            playerManager.SetUI_PlayerStatusViewActive(true);
        }
    }
    public void OpenRewardScene()
    {
        //報酬シーンのUIを選べる状態にする
        SetSelectUI_IsActive();

        //アイテム生成
        GenerateItem();
    }
    public void CloseRewardScene()
    {
        ////選択可能状態を解除したいものをリストにまとめる
        List<UI_Base> uiList = new List<UI_Base>();

        if (itemContainer != null)
        {
            List<UI_ItemSlot_V2> tempList = itemContainer.GetItem_DisplayUI_List();
            for (int i = 0; i < tempList.Count; ++i)
            {
                uiList.Add(tempList[i]);//リストに追加
            }
        }

        //売却UIを追加
        uiList.Add(UI_BuySlot);
        uiList.Add(UI_CloseEventSlot);
      
        if (playerManager != null)
        {
            //プレイヤーストレージのUIを追加
            ItemContainer container = playerManager.GetItemContainer();
            if (container != null)
            {
                List<UI_ItemSlot_V2> tempList = container.GetItem_DisplayUI_List();
                for (int i = 0; i < tempList.Count; ++i)
                {
                    uiList.Add(tempList[i]);//リストに追加
                }
            }
        }

        //PlayerのコンテナのUIを追加
        GameObject Player1 = GameManager.Instance.GetPlayer(1);
        GameObject Player2 = GameManager.Instance.GetPlayer(2);
        PlayerController_3d Player1comp = null;
        PlayerController_3d Player2comp = null;
        if (Player1 != null) Player1comp = Player1.GetComponent<PlayerController_3d>();
        if (Player2 != null) Player2comp = Player2.GetComponent<PlayerController_3d>();
        if (Player1comp != null)
        {
            ItemContainer itemContainer = Player1comp.GetItemContainer();
            if (itemContainer != null)
            {
                List<UI_ItemSlot_V2> tempList = itemContainer.GetItem_DisplayUI_List();
                for (int i = 0; i < tempList.Count; ++i)
                {
                    uiList.Add(tempList[i]);
                }
            }
        }
        if (Player2comp != null)
        {
            ItemContainer itemContainer = Player2comp.GetItemContainer();
            if (itemContainer != null)
            {
                List<UI_ItemSlot_V2> tempList = itemContainer.GetItem_DisplayUI_List();
                for (int i = 0; i < tempList.Count; ++i)
                {
                    uiList.Add(tempList[i]);
                }
            }
        }

        //除外する
        GameManager.Instance.RemoveSelectSlot(uiList);

        //UIを非表示にする
        for (int i = 0; i < uiList.Count; ++i)
        {
            if (uiList != null)
            {
                uiList[i].gameObject.SetActive(false);
            }
        }
        if (playerManager != null)
        {
            //プレイヤーステータスのUIを非表示
            playerManager.SetUI_PlayerStatusViewActive(false);
        }
    }

    //sauceリストの内容をTargetリストにコピーする関数(重複回避)
    public List<T> MergeLists<T>(List<T> target, List<T> source)
    {
        foreach (var item in source)
        {
            if (!target.Contains(item))
            {
                target.Add(item);
            }
        }

        return target;
    }
}
