using UnityEngine;

public class UI_ItemSlot_V2_S : UI_ItemSlot_V2
{
    [SerializeField] GameObject SubTab;//サブタブ
    public override void SetSelectingHighlight_UI(bool _flag)
    {
        IsSelectiveFalg = _flag;
        SetHighlight_UI(_flag);

        if (SubTab != null)
        {
            //選択状態かつアイテムセット状態
            if (IsSelectiveFalg && IsSetedItem)
            {
                SubTab.SetActive(true);
            }
            else
            {
                SubTab.SetActive(false);
            }
        }
    }
}
