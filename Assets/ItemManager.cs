using UnityEngine;

public class ItemManager : MonoBehaviour
{
    //アイテムを入れてる場所を記録するデータ型
    public struct ItemSlot
    {
        public ItemContainer Container;
        public int Index;

        public ItemSlot(ItemContainer container, int index)
        {
            Container = container;
            Index = index;
        }
    }

    private ItemSlot HoldSlot;//保留中のスロット
                              //private ItemSlot SelectSlot;//選択中のスロット

    public void TradeItem(ItemContainer _Container, int _Index)
    {
        //引数の有効性チェック
        if (_Container == null) return;


        //保留中のスロットがなかったら登録
        if (HoldSlot.Container == null)
        {
            HoldSlot.Container = _Container;
            HoldSlot.Index = _Index;

            //点滅設定
            UI_Base ui = HoldSlot.Container.GetItem_DisplayUI(HoldSlot.Index);
            if (ui != null)
            {
                GameManager.Instance.SetHoldingUI(ui);
            }
        }
        else //保留中のスロットがある場合は入れ替えを実施
        {
            //仮として保留から取得し記録
            EquipmentItem_Base tempItem = HoldSlot.Container.GetItem(HoldSlot.Index);

            //代入
            HoldSlot.Container.SetItem(HoldSlot.Index, _Container.GetItem(_Index));
            _Container.SetItem(_Index, tempItem);

            //保留をリセット
            HoldSlot.Container = null;

            //点滅解除
            GameManager.Instance.SetHoldingUI(null);
        }
    }
    //アイテムの売却
    public void SaleItem()
    {
        //保留アイテムを売却
        if (HoldSlot.Container != null)
        {
            //保留からアイテムを取得
            EquipmentItem_Base Item = HoldSlot.Container.GetItem(HoldSlot.Index);

            if (Item != null)
            {
                //価格を取得
                int cost = Item.GetSaleCost();

                //保留スロットのアイテムを削除
                HoldSlot.Container.SetItem(HoldSlot.Index, null);

                //残高に価格を加算
                GameManager.Instance.SetMoney(GameManager.Instance.GetMoney() + cost);

                //保留をリセット
                HoldSlot.Container = null;

                //点滅解除
                GameManager.Instance.SetHoldingUI(null);
            }
        }
    }
}
