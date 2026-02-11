using UnityEngine;
using System.Collections.Generic;

public class UI_FastItemSelector : MonoBehaviour
{
    [SerializeField] ItemContainer itemContainer;

    private void Awake()
    {
        //アイテムコンテナ取得
        itemContainer = GetComponent<ItemContainer>();
    }
    void Start()
    {
        //選択時のイベント登録
        itemContainer.SetClickEvent(OnClick_UI_SetMainWeapon);

        //選択可能状態にする
        SetSelectiveSlotUI();
    }

    void Remove()
    {
        //通常プレイにもどす
        GameManager.Instance.Event_PlayGame();

        //UIの選択状態を解除
        RemoveSelectiveSlotUI();

        //メニューUIを削除する
        GameManager.Instance.ActivateOnlyShowDisplayUI(null);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
        }
    }

    //UIスロットを選択可能な状態にする
    private void SetSelectiveSlotUI()
    {
        //他の選択肢を無効にしておく
        GameManager.Instance.RemoveSelectSlot_all();

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

        //登録
        GameManager.Instance.SetSelectSlot_isSelective(uiList);

    }
    //UIスロットを選択可能な状態を解除する
    private void RemoveSelectiveSlotUI()
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

        //登録解除
        GameManager.Instance.RemoveSelectSlot(uiList);
    }
    public void　OnClick_UI_SetMainWeapon(UI_Base _ui)
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

                    //選択したアイテムをマネージャーに登録
                    if (itemList[i] is Weapon)
                    {
                        GameManager.Instance.SetMainWeapon((Weapon)itemList[i]);
                    }

                    //選択メニューUI削除
                    Remove();

                    return;
                }
            }
        }
    }
}
