using System.Collections.Generic;
using UnityEngine;

public class UI_StorageInventory : MonoBehaviour
{
    [SerializeField] private List<GameObject> SelectSlotList;
    public List<GameObject> GetSelectSlotList()
    {
        return SelectSlotList;
    }
}
