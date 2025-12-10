using UnityEngine;

public class UI_RewardSlot : UI_SelectSlot
{
    [SerializeField] UI_RewardScene RewardScene;
    protected override void DecideAction()
    {
        Debug.Log("ïÒèVÇëIÇ—Ç‹ÇµÇΩ");
        if (RewardScene != null)
        {

        }
    }

    public void SetRewardScene(UI_RewardScene _RewardScene)
    {
        RewardScene = _RewardScene;
    }
}
