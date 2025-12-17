using UnityEngine;
using UnityEngine.UI;

public class UI_RewardSlot : UI_SelectSlot
{
    [SerializeField] int SaleCost;//売値
    [SerializeField] Text TextSaleCost;//売値を表示するテキスト
    public override void DecideAction()
    {
        //Debug.Log("報酬を選びました");
        if (RewardScene != null)
        {
            bool Result = false;

            //残高取得
            int Money = GameManager.Instance.GetMoney();

            Money -= SaleCost;

            //赤字にならないか確認
            if (Money >= 0)
            {
                //購入可能

                if(Item is Accessory)
                {
                    //アクセサリーは別


                }
                else
                {
                    //通常アイテム

                    //ストレージにアイテムを追加
                    Result = RewardScene.TransferItem_toStorage(Item);
                }
                   
            }
            else
            {
                Debug.Log("残高がたりません");
            }

            

            //入れ変えに成功した場合はこちらのアイテムを外す
            if (Result == true)
            {
                //おつりを代入
                GameManager.Instance.SetMoney(Money);
                
                Item = null;
            }
        }
    }

    public override void Update_Display()
    {
        if (Item != null)
        {
            if (ImageComp != null) ImageComp.sprite = Item.GetItemSprite();//画像を設定
            if (TextNameComp != null) TextNameComp.text = Item.GetItemName();//アイテム名を設定
            if (TextExpComp != null) TextExpComp.text = Item.GetItemExplanation();//アイテム説明を設定

            SaleCost = Item.GetSaleCost();
            if (TextSaleCost != null) TextSaleCost.text = SaleCost.ToString() + "G";//アイテムの売値を設定
        }
        else
        {
            //アイテムが無効である場合
            if (ImageComp != null) ImageComp.sprite = null;//
            if (TextNameComp != null) TextNameComp.text = " ";//
            if (TextExpComp != null) TextExpComp.text = " ";

            SaleCost = 0;
            if (TextSaleCost != null) TextSaleCost.text = " ";
        }
    }
}
