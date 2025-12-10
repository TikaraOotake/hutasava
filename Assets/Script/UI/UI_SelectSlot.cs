using UnityEngine;
using UnityEngine.UI;

public class UI_SelectSlot : MonoBehaviour
{
    //選択項目のUI

    [SerializeField]
    protected EquipmentItem_Base Item;//登録されているItem
    protected EquipmentItem_Base Item_old;//前フレームの登録されているItem
    [SerializeField] protected bool SelectingFlag;//選択中のフラグ

    //表示させるためのもの
    [SerializeField] protected Image ImageComp;//イメージ表示用
    [SerializeField] protected Text TextNameComp;//名前表示用
    [SerializeField] protected Text TextExpComp;//説明文表示用

    [SerializeField] protected Image SelectingLightComp;//選択中に発光させる用

    protected Color NeutralColor;//通常色

    [SerializeField] protected UI_RewardScene RewardScene;
    

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
        return Item;
    }
    public void SetSelectingFlag()
    {
        SelectingFlag = true;
    }

    protected void Update_Display()
    {
        if (Item != null)
        {
            if (ImageComp != null) ImageComp.sprite = Item.GetItemSprite();//画像を設定
            if (TextNameComp != null) TextNameComp.text = Item.GetItemName();//アイテム名を設定
            if (TextExpComp != null) TextExpComp.text = Item.GetItemExplanation();//アイテム説明を設定
        }
        else
        {
            //アイテムが無効である場合
            if (ImageComp != null) ImageComp.sprite = null;//
            if (TextNameComp != null) TextNameComp.text = " ";//
            if (TextExpComp != null) TextExpComp.text = " ";
        }
    }

    //選択決定時の挙動
    public virtual void DecideAction()
    {
        if (RewardScene != null)
        {
            RewardScene.ExchangeItem(this);
        }
    }
    public void SetRewardScene(UI_RewardScene _RewardScene)
    {
        RewardScene = _RewardScene;
    }
}
