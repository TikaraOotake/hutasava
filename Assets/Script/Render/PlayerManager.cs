using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //Player
    [SerializeField] List<GameObject> PlayerList = new List<GameObject>();//プレイヤーを格納するためのリスト

    [SerializeField] GameObject Player1;
    [SerializeField] GameObject Player2;

    [SerializeField] UI_PlayerStatusView ui_PlayerStatusView1;
    [SerializeField] UI_PlayerStatusView ui_PlayerStatusView2;

    //アイテムを格納するコンテナ
    [SerializeField] ItemContainer itemContainer;

    //絆効果を格納するリスト
    [SerializeField] List<BondEffect> bondEffectList = new List<BondEffect>();

    [SerializeField] BondEffect testBond;



    void Start()
    {
        if (Player1 != null) GameManager.Instance.GetPlayer(1);
        if (Player2 != null) GameManager.Instance.GetPlayer(2);

        if (itemContainer != null)
        {
            //クリック時のEventを設定
            itemContainer.SetClickEvent(itemContainer.TradeItem);

            //選択可能状態にしたい選択肢を登録
            GameManager.Instance.SetSelectSlot_isSelective(itemContainer.GetItem_DisplayUI_List());
        }
        else
        {
            Debug.Log("アイテムコンテナが生成されていません");
        }


        if (itemContainer != null)
        {
            List<UI_ItemSlot_V2> uiList = itemContainer.GetItem_DisplayUI_List();
            //選択可能状態にしたい選択肢を登録
            GameManager.Instance.SetSelectSlot_isSelective(uiList);
        }
    }
    private void Update()
    {
        if (testBond != null)
        {
            testBond.Update_BondEffect();
        }
    }
    public bool SetItem(int _Index, EquipmentItem_Base _Item)
    {
        bool Result = false;
        if (itemContainer != null)
        {
            Result = itemContainer.SetItem(_Index, _Item);
        }

        return Result;
    }

    public ItemContainer GetItemContainer()
    {
        return itemContainer;
    }

    public void Update_PlayerStatusView()
    {
        //Debug.Log("プレイヤーステータスの表示を更新");

        int HP = 0;
        int Attack = 0;
        int Speed = 0;
        if (Player1 != null)
        {
            PlayerController_3d playerController = Player1.GetComponent<PlayerController_3d>();
            if (playerController != null)
            {
                HP = (int)playerController.GetHealthPointMax_Result();
                Attack = (int)playerController.GetAttackPoint_Result();
                Speed = (int)playerController.GetMoveSpeedPoint_Result();
            }
            else
            {
                Debug.Log("プレイヤー1が見つかりません");
            }
        }
        if (ui_PlayerStatusView1 != null) ui_PlayerStatusView1.SetPlayerStatus(HP, Attack, Speed);


        if (Player2 != null)
        {
            PlayerController_3d playerController = Player2.GetComponent<PlayerController_3d>();
            if (playerController != null)
            {
                HP = (int)playerController.GetHealthPointMax_Result();
                Attack = (int)playerController.GetAttackPoint_Result();
                Speed = (int)playerController.GetMoveSpeedPoint_Result();
            }
        }
        if (ui_PlayerStatusView2 != null) ui_PlayerStatusView2.SetPlayerStatus(HP, Attack, Speed);
    }
    public void SetUI_PlayerStatusViewActive(bool _flag)
    {
        if (ui_PlayerStatusView1 != null) ui_PlayerStatusView1.gameObject.SetActive(_flag);
        if (ui_PlayerStatusView2 != null) ui_PlayerStatusView2.gameObject.SetActive(_flag);
    }

    //プレイヤーにアクセサリをセット
    public void SetAccessory_Player(int _PlayerNam,Accessory _Accessory)
    {
        if (_PlayerNam == 1 && Player1 != null)
        {
            PlayerController_3d player = Player1.GetComponent<PlayerController_3d>();//取得
            if (player != null) player.SetAccessory(_Accessory);//アクセをセット
        }
        else if (_PlayerNam == 2 && Player2 != null)
        {
            PlayerController_3d player = Player2.GetComponent<PlayerController_3d>();//取得
            if (player != null) player.SetAccessory(_Accessory);//アクセをセット
        }
        else
        {
            Debug.Log("アクセの装備に失敗しました");
        }
    }

    //絆効果のセット
    public void SetBondEffect()
    {
        //初期化
        for (int i = 0; i < bondEffectList.Count; ++i)
        {
            if (bondEffectList[i] != null)//有効性確認
            {
                bondEffectList[i].SetLevel(0);
            }
        }

        List<EquipmentItem_Base> itemList1 = new List<EquipmentItem_Base>();
        List<EquipmentItem_Base> itemList2 = new List<EquipmentItem_Base>();

        List<EquipmentItem_Base> itemList = new List<EquipmentItem_Base>();

        //取得
        if (Player1 != null)
        {
            ItemContainer itemContainer1 = Player1.GetComponent<ItemContainer>();
            if (itemContainer1 != null) itemList1 = itemContainer1.GetItemList();
        }
        if (Player2 != null)
        {
            ItemContainer itemContainer2 = Player2.GetComponent<ItemContainer>();
            if (itemContainer2 != null) itemList2 = itemContainer2.GetItemList();
        }

        //リスト
        for (int i = 0; i < itemList1.Count; ++i)
        {
            itemList.Add(itemList1[i]);
        }
        for (int i = 0; i < itemList2.Count; ++i)
        {
            itemList.Add(itemList2[i]);
        }

        //絆効果を集計
        for (int i = 0; i < itemList.Count; ++i)
        {
            if (itemList[i] != null)//有効確認
            {
                if (itemList[i] is Weapon)
                {
                    Weapon weapon = (Weapon)itemList[i];//型変換
                    
                }
            }
        }

    }
}
