using UnityEngine;
using UnityEngine.UI;

public class UI_SelectSlot : MonoBehaviour
{
    //選択項目のUI

    [SerializeField]
    private EquipmentItem_Base Item;//登録されているItem
    private EquipmentItem_Base Item_old;//前フレームの登録されているItem
    [SerializeField] private bool SelectingFlag;//選択中のフラグ

    //表示させるためのもの
    [SerializeField] private Image ImageComp;//イメージ表示用
    [SerializeField] private Text TextNameComp;//名前表示用
    [SerializeField] private Text TextExpComp;//説明文表示用

    [SerializeField] private Image SelectingLightComp;//選択中に発光させる用

    private Color NeutralColor;//通常色

    void Start()
    {
        Update_Display();

        if (SelectingLightComp != null)
        {
            NeutralColor = SelectingLightComp.color;//通常色を記憶
        }
    }

    // Update is called once per frame
    void Update()
    {
        //アイテムの情報が変わったらUI更新
        if (Item != Item_old)
        {
            Update_Display();
        }

        
        if (SelectingLightComp != null)
        {
            if (SelectingFlag)
            {
                SelectingLightComp.color = Color.yellow;//選択中なら発光
            }
            else
            {
                SelectingLightComp.color = NeutralColor;//通常色
            }
        }

        Item_old = Item;
        SelectingFlag = false;
    }
    public void SetItem(EquipmentItem_Base _Item)
    {
        if (Item != null)
        {
            Item = null;
        }

        if (_Item != null)
        {
            Item = Instantiate(_Item);
        }
        else
        {
            Item = null;
        }
    }
    public EquipmentItem_Base GetItem()
    {
        if (Item != null)
        {
            return Instantiate(Item);
        }
        else
        {
            return null;
        }
    }
    public void SetSelectingFlag()
    {
        SelectingFlag = true;
    }

    private void Update_Display()
    {
        if (Item != null)
        {
            if (ImageComp != null) ImageComp.sprite = Item.GetItemSprite();//画像を設定
            if (TextNameComp != null) TextNameComp.text = Item.GetItemName();//アイテム名を設定
            if (TextExpComp != null) TextExpComp.text = Item.GetItemExplanation();//アイテム説明を設定
        }
    }
}
