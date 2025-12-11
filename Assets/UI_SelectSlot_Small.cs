using UnityEngine;
//using static UnityEditor.Progress;

public class UI_SelectSlot_Small : UI_SelectSlot
{
    [SerializeField] private GameObject SubColumn;//サブ欄のオブジェクト

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
}
