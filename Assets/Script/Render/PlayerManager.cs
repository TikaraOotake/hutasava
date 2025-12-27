using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class PlayerManager : MonoBehaviour
{
    //Player
    [SerializeField] List<GameObject> PlayerList;//プレイヤーを格納するためのリスト

    //アイテムを格納するコンテナ
    [SerializeField] ItemContainer itemContainer;

   

    void Start()
    {
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
}
