using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI_RewardScene : MonoBehaviour
{
    [SerializeField] private UI_Inventory_Player Player1Inventory;
    [SerializeField] private UI_Inventory_Player Player2Inventory;
    [SerializeField] private UI_Inventory Inventory;


    public List<Transform> uiObjects;   // 選択可能UI
    [SerializeField] private List<GameObject> UI_SelectList;   // 選択可能UI
    [SerializeField] private int currentIndex = 0;        // 現在選択中のインデックス

    [SerializeField] private List<GameObject> UI_RewardSlotList;//報酬スロットのリスト

    void Start()
    {
        

        //報酬スロットと紐づけ
        for (int i = 0; i < UI_RewardSlotList.Count; ++i)
        {
            UI_RewardSlot rewardSlot = UI_RewardSlotList[i].GetComponent<UI_RewardSlot>();

            if (rewardSlot != null)
            {
                rewardSlot.SetRewardScene(this);//自身を登録しておく
            }
        }

        //リストを取得
        List<GameObject> Player1InventoryList = new List<GameObject>();
        List<GameObject> Player2InventoryList = new List<GameObject>();
        List<GameObject> InventoryList = new List<GameObject>();
        if (Player1Inventory != null) Player1InventoryList = Player1Inventory.GetPlayerItemSlot();
        if (Player2Inventory != null) Player2InventoryList = Player2Inventory.GetPlayerItemSlot();
        if (Inventory != null) InventoryList = Inventory.GetSelectSlotList();

        //PrintList("RewardSlot", UI_RewardSlotList);
        //PrintList("Player1", Player1InventoryList);
        //PrintList("Player2", Player2InventoryList);

        //選択リストに登録
        UI_SelectList = MergeLists(UI_SelectList, UI_RewardSlotList);
        UI_SelectList = MergeLists(UI_SelectList, Player1InventoryList);
        UI_SelectList = MergeLists(UI_SelectList, Player2InventoryList);
        UI_SelectList = MergeLists(UI_SelectList, InventoryList);


        // 保険：範囲外の時は0に戻す
        currentIndex = Mathf.Clamp(currentIndex, 0, UI_SelectList.Count - 1);
        if (currentIndex >= 0)
        {
            HighlightUI(UI_SelectList[currentIndex]);
        }
    }

    void Update()
    {
        SelectNext();

        //HighlightUI(UI_SelectList[currentIndex].transform);

        int inputNum = 0;
        if (Input.GetKeyDown(KeyCode.W))
        {
            ++inputNum;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            --inputNum;
        }

        //選択中の選択肢を発光
        if (currentIndex >= 0)
        {
            HighlightUI(UI_SelectList[currentIndex]);
        }

    }

    Vector2 GetInputDir()
    {
        if (Input.GetKeyDown(KeyCode.W)) return Vector2.up;
        if (Input.GetKeyDown(KeyCode.S)) return Vector2.down;
        if (Input.GetKeyDown(KeyCode.A)) return Vector2.left;
        if (Input.GetKeyDown(KeyCode.D)) return Vector2.right;
        return Vector2.zero;
    }

    void SelectNext()
    {
        Vector2 inputDir = GetInputDir();
        if (inputDir == Vector2.zero) return;

        Transform current = UI_SelectList[currentIndex].transform; // ←番号で取得

        Transform best = null;
        float bestScore = float.MaxValue;
        int bestIndex = currentIndex;

        for (int i = 0; i < UI_SelectList.Count; i++)
        {
            if (i == currentIndex) continue;

            Vector2 diff = (Vector2)UI_SelectList[i].transform.position - (Vector2)current.position;

            // 入力と逆方向は除外
            if (Vector2.Dot(inputDir, diff.normalized) < 0.2f) continue;

            float angle = Vector2.Angle(inputDir, diff);
            float dist = diff.magnitude;
            float score = angle * 1.0f + dist * 0.2f;

            if (score < bestScore)
            {
                bestScore = score;
                best = UI_SelectList[i].transform;
                bestIndex = i;  // ← インデックスを記録
            }
        }

        if (best != null)
        {
            currentIndex = bestIndex;       // ←選択中インデックスを更新
            //HighlightUI(UI_SelectList[currentIndex].transform);
        }
    }

    void HighlightUI(GameObject _UI_obj)
    {
        //Debug.Log("Selected index: " + currentIndex + " -> " + tr.name);
        // ここにハイライト処理を書く

        UI_SelectSlot SelectSlot = _UI_obj.GetComponent<UI_SelectSlot>();

        if (SelectSlot != null)
        {
            SelectSlot.SetSelectingFlag();
        }
    }

    //sauceリストの内容をTargetリストにコピーする関数(重複回避)
    public List<T> MergeLists<T>(List<T> target, List<T> source)
    {
        foreach (var item in source)
        {
            if (!target.Contains(item))
            {
                target.Add(item);
            }
        }

        return target;
    }


    public List<GameObject> MergeLists(List<GameObject> target, List<GameObject> source)
    {
        foreach (var obj in source)
        {
            if (obj == null) continue;     // null対策
            if (!target.Contains(obj))     // 重複チェック
                target.Add(obj);
        }

        return target;
    }

    void PrintList(string name, List<GameObject> list)
    {
        foreach (var o in list)
        {
            Debug.Log($"{name}: {o?.name}");
        }
    }
    public void SetItem()
    {

    }
}
