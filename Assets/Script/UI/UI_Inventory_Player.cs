using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI_Inventory_Player : MonoBehaviour
{
    [SerializeField] private List<GameObject> PlayerItemSlot;
    [SerializeField] private List<Accessory> PlayerAccessoryList;

    [SerializeField] private PlayerController_3d Player;

    private void Start()
    {
        if(Player)
        {
            Player.CalcuStatus();
        }
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
}
