using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI_Inventory_Player : MonoBehaviour
{
    [SerializeField] private List<GameObject> PlayerItemSlot;
    [SerializeField] private List<Accessory> PlayerAccessoryList;

    [SerializeField] private PlayerController_3d Player;

    [SerializeField] private bool SelectingFlag;//選択フラグ
    [SerializeField] protected Image SelectingLightComp;//選択中に発光させる用
    protected Color NeutralColor;//通常色

    [SerializeField] Text HealthView;
    [SerializeField] Text AtkViw;
    [SerializeField] Text SpeedViw;

    private void Start()
    {
        if (Player)
        {
            Player.CalcuStatus();
        }

        if (SelectingLightComp != null)
        {
            NeutralColor = SelectingLightComp.color;//通常色を記憶
        }
    }
    private void Update()
    {
        if (SelectingLightComp != null)
        {
            if (SelectingFlag)
            {
                Color color = Color.yellow;//選択中なら発光
                color.a = NeutralColor.a;
                SelectingLightComp.color = color;
            }
            else
            {
                SelectingLightComp.color = NeutralColor;//通常色
            }
        }

        SelectingFlag = false;
    }
    public List<GameObject> GetPlayerItemSlot()
    {
        return PlayerItemSlot;
    }
    public void SetAccessory(Accessory _accessory)
    {
        PlayerAccessoryList.Add(_accessory);

        //プレイヤーのステータスを再計算
        if (Player != null)
        {
            Player.CalcuStatus();
        }
    }
    public List<Accessory> GetAccessoryList()
    {
        return PlayerAccessoryList;
    }
    public void SetSelectingFlag(bool _flag)
    {
        SelectingFlag = _flag;
    }
    public void SetSelectingFlag()
    {
        SelectingFlag = true;
    }
}
