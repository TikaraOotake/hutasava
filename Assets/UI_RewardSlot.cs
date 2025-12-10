using UnityEngine;

public class UI_RewardSlot : UI_SelectSlot
{
    
    public override void DecideAction()
    {
        //Debug.Log("報酬を選びました");
        if (RewardScene != null)
        {
            bool Result = false;

            Result = RewardScene.TransferItem_toStorage(Item);
            
            //入れ変えに成功した場合はこちらのアイテムを外す
            if(Result==true)
            {
                Item = null;
            }
        }
    }

    
}
