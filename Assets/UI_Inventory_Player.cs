using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI_Inventory_Player : MonoBehaviour
{
    [SerializeField] private List<GameObject> PlayerItemSlot;

    public List<GameObject> GetPlayerItemSlot()
    {
        return PlayerItemSlot;
    }
}
