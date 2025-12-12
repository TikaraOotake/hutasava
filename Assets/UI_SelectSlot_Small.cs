using UnityEngine;
using UnityEngine.UI;
//using static UnityEditor.Progress;

public class UI_SelectSlot_Small : UI_SelectSlot
{
    [SerializeField] private GameObject SubColumn;//サブ欄のオブジェクト
    [SerializeField]
    protected Text TextLevelComp;//レベルを表示させるためのテキスト


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

        //選択中であればサブ欄を表示
        if (SelectingFlag == true && Item != null)
        {
            SubColumn.SetActive(true);//表示
        }
        else
        {
            SubColumn.SetActive(false);//非表示
        }

        Item_old = Item;
        SelectingFlag = false;
    }

    public override void Update_Display()
    {
        if (Item != null)
        {
            if (ImageComp != null) ImageComp.sprite = Item.GetItemSprite();//画像を設定
            if (TextNameComp != null) TextNameComp.text = Item.GetItemName();//アイテム名を設定
            if (TextExpComp != null) TextExpComp.text = Item.GetItemExplanation();//アイテム説明を設定
            if (TextLevelComp != null)
            {
                Weapon weapon = (Weapon)Item;
                int Level = weapon.GetWeaponLevel();
                if (Level > 0)
                {
                    TextLevelComp.text = "Lv." + Level.ToString();
                }
                else
                {
                    TextLevelComp.text = " ";
                }
            }
        }
        else
        {
            //アイテムが無効である場合
            if (ImageComp != null) ImageComp.sprite = null;//
            if (TextNameComp != null) TextNameComp.text = " ";//
            if (TextExpComp != null) TextExpComp.text = " ";
            if (TextLevelComp != null) TextLevelComp.text = " ";
        }
    }
}
