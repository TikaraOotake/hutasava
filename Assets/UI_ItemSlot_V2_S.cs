using UnityEngine;

public class UI_ItemSlot_V2_S : UI_ItemSlot_V2
{
    [SerializeField] GameObject SubTab;//サブタブ
    public override void SetSelectingHighlight_UI(bool _flag)
    {
        IsSelectiveFalg = _flag;
        SetHighlight_UI(_flag);

        //選択中にUIを拡大させる
        if(IsSelectiveFalg)
        {
            transform.localScale = UI_BaseScale * SelectUpScaleRate;//大きさを拡大
        }
        else
        {
            transform.localScale = UI_BaseScale;//大きさを通常に
        }

        //サブタブの表示条件
        if (SubTab != null)
        {
            //選択状態かつアイテムセット状態
            if (IsSelectiveFalg && IsSetedItem)
            {
                SubTab.SetActive(true);//表示

                //タブがはみ出ないようにスロットの位置に合わせてずらす
                RectTransform SlotRectTransform = this.GetComponent<RectTransform>();
                RectTransform TabRectTransform = SubTab.GetComponent<RectTransform>();
                if (SlotRectTransform != null && TabRectTransform != null)
                {
                    Vector3 pos = TabRectTransform.localPosition;//タブの座標取得
                    if (SlotRectTransform.localPosition.y < 0)
                    {
                        if (TabRectTransform.localPosition.y < 0) pos.y *= -1;//反転 
                    }
                    else
                    {
                        if (TabRectTransform.localPosition.y > 0) pos.y *= -1;//反転 
                    }
                    TabRectTransform.localPosition = pos;//座標代入
                }
            }
            else
            {
                SubTab.SetActive(false);//非表示
            }
        }
    }
}
