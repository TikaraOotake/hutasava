using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class PlayerManager : MonoBehaviour
{
    //Player
    [SerializeField] List<GameObject> PlayerList;//プレイヤーを格納するためのリスト

    [SerializeField] GameObject Player1;
    [SerializeField] GameObject Player2;

    [SerializeField] UI_PlayerStatusView ui_PlayerStatusView1;
    [SerializeField] UI_PlayerStatusView ui_PlayerStatusView2;

    //アイテムを格納するコンテナ
    [SerializeField] ItemContainer itemContainer;

   

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
}
