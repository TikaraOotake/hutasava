using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI_RewardScene : MonoBehaviour
{
    [SerializeField] private UI_Inventory_Player Player1Inventory;
    [SerializeField] private UI_Inventory_Player Player2Inventory;
    [SerializeField] private UI_StorageInventory Inventory;


    public List<Transform> uiObjects;   // 選択可能UI
    [SerializeField] private List<GameObject> UI_SelectList;   // 選択可能UI
    [SerializeField] private int currentIndex = 0;        // 現在選択中のインデックス

    [SerializeField] private List<GameObject> UI_RewardSlotList;//報酬スロットのリスト

    private UI_SelectSlot OnHoldSelectSlot;//保留スロット(入れ替え用)

    private float JoyStickInputLength_old;

    [SerializeField]
    private bool MainSelectUI_LockFalg;//メインUIの選択ロックのフラグ
    [SerializeField]
    private int SelectPlayerNum = 0;

    [SerializeField] private Accessory tempAcce;

    void Start()
    {
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

        //報酬スロットと紐づけ
        for (int i = 0; i < UI_SelectList.Count; ++i)
        {
            GameObject obj = UI_SelectList[i];
            if (obj != null)
            {
                UI_SelectSlot Slot = obj.GetComponent<UI_SelectSlot>();

                if (Slot != null)
                {
                    Slot.SetRewardScene(this);//自身を登録しておく
                }
            }
        }


        //報酬スロットのアイテムを作成
        GenerateItem();


        // 保険：範囲外の時は0に戻す
        currentIndex = Mathf.Clamp(currentIndex, 0, UI_SelectList.Count - 1);
        if (currentIndex >= 0)
        {
            HighlightUI(UI_SelectList[currentIndex]);
        }
    }

    void Update()
    {
        if(!MainSelectUI_LockFalg)
        {
            SelectNext();

            //HighlightUI(UI_SelectList[currentIndex].transform);

            //決定入力
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Joystick1Button1))
            {
                GameObject obj = UI_SelectList[currentIndex];
                if (obj != null)
                {
                    UI_SelectSlot slot = obj.GetComponent<UI_SelectSlot>();
                    if (slot != null)
                    {
                        slot.DecideAction();
                    }
                    else
                    {
                        Debug.Log("セレクトスロットがありません");
                    }
                }
                else
                {
                    Debug.Log("オブジェクトが無効です");
                }
            }

            //選択中の選択肢を発光
            if (currentIndex >= 0)
            {
                HighlightUI(UI_SelectList[currentIndex]);//選択スロット
            }
            if (OnHoldSelectSlot != null)
            {
                HighlightUI(OnHoldSelectSlot.gameObject);//保留スロット
            }
        }
        else
        {
            int input = 0;
            if (Input.GetKeyDown(KeyCode.A))
            {
                input += -1;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                input += +1;
            }
            if (input == 0)
            {
                Vector2 vec = GetInputDir();
                if (vec.x > 0)
                {
                    input += 1;
                }
                else if (vec.x < 0)
                {
                    input -= 1;
                }
            }

            if (input > 0)
            {
                SelectPlayerNum = 2;
            }
            else if (input < 0)
            {
                SelectPlayerNum = 1;
            }

            //決定入力
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Joystick1Button1))
            {
                if (tempAcce != null)
                {
                    if (SelectPlayerNum == 1)
                    {
                        Player1Inventory.SetAccessory(tempAcce);
                    }
                    else if (SelectPlayerNum == 2)
                    {
                        Player2Inventory.SetAccessory(tempAcce);
                    }
                }


                //戻す
                MainSelectUI_LockFalg = false;
            }

            if (SelectPlayerNum == 1)
            {
                HighlightUI(Player1Inventory.gameObject);
            }
            else if (SelectPlayerNum == 2)
            {
                HighlightUI(Player2Inventory.gameObject);
            }
        }
    }

    Vector2 GetInputDir()
    {
        if (Input.GetKeyDown(KeyCode.W)) return Vector2.up;
        if (Input.GetKeyDown(KeyCode.S)) return Vector2.down;
        if (Input.GetKeyDown(KeyCode.A)) return Vector2.left;
        if (Input.GetKeyDown(KeyCode.D)) return Vector2.right;

        Vector2 InputValue = Vector2.zero;

        InputValue.x = Input.GetAxis("Horizontal");
        InputValue.y = Input.GetAxis("Vertical");

        const float threshold = 0.8f;//閾値
        bool flag = false;
        float InputLength = Vector2.Distance(InputValue, Vector2.zero);

        if (JoyStickInputLength_old < threshold && InputLength >= threshold)
        {
            flag = true;
        }

        JoyStickInputLength_old = InputLength;

        if (flag)
        {
            return InputValue.normalized;
        }
        else
        {
            return Vector2.zero;
        }
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

        UI_Inventory_Player _Inventory_Player = _UI_obj.GetComponent<UI_Inventory_Player>();
        if (_Inventory_Player != null)
        {
            _Inventory_Player.SetSelectingFlag();
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
    public void GenerateItem()
    {
        for (int i = 0; i < UI_RewardSlotList.Count; ++i)
        {
            UI_SelectSlot slot = UI_RewardSlotList[i].GetComponent<UI_SelectSlot>();
            if (slot != null)
            {
                //ランダムなアイテムデータを渡す
                slot.SetItem(GameManager.Instance.GetRandCopyItemData());
            }
        }
    }
    public void SetItem()
    {

    }
    public bool TransferItem_toStorage(EquipmentItem_Base _Itemdata)
    {
        List<GameObject> InventoryList = new List<GameObject>();
        if (Inventory != null) InventoryList = Inventory.GetSelectSlotList();

        for (int i = 0; i < InventoryList.Count; ++i)
        {
            GameObject obj = InventoryList[i];
            if (obj != null)
            {
                UI_SelectSlot slot = obj.GetComponent<UI_SelectSlot>();
                if (slot != null)
                {
                    //既に入っているアイテムを取得
                    EquipmentItem_Base item = slot.GetItem();

                    //アイテムが空であれば引き数のアイテムを代入して終了
                    if (item == null)
                    {
                        slot.SetItem(_Itemdata);
                        return true;
                    }
                }
            }
        }

        return false;
    }
    public void ExchangeItem(UI_SelectSlot _slot)
    {
        //引き数スロットの有効性確認
        if (_slot == null) return;//無効なため終了

        //保留変数を確認
        if (OnHoldSelectSlot != null)
        {
            //保留が選択済みなので交換開始

            //保留と同じものを選択していないか確認
            if (OnHoldSelectSlot == _slot)
            {
                //同じものを指定しているため保留を解除して終了
                OnHoldSelectSlot = null;
                return;
            }

            //保留に入っているアイテムと同じ派生クラスか確認
            UI_SelectSlot holdSlot = OnHoldSelectSlot.GetComponent<UI_SelectSlot>();
            UI_SelectSlot arguSlot = _slot.GetComponent<UI_SelectSlot>();
            if (holdSlot && arguSlot)//コンポの有効性を確認
            {
                EquipmentItem_Base holdItem = holdSlot.GetItem();
                EquipmentItem_Base arguItem = arguSlot.GetItem();
                if (holdItem && arguItem)//アイテムの有効性を確認
                {
                    if (holdItem.GetType() == arguItem.GetType())//同じ派生クラスか確認
                    {
                        //型変換
                        Weapon holdWeapon = (Weapon)holdItem;
                        Weapon arguWeapon = (Weapon)arguItem;
                        if (holdWeapon.GetWeaponLevel() == arguWeapon.GetWeaponLevel())
                        {
                            //Debug.Log("同じ種類の同じレベルです");
                            //武器合成
                            arguWeapon.SetWeaponLevel(arguWeapon.GetWeaponLevel() + 1);//レベルを1上げる

                            //スロットの表示を更新
                            arguSlot.Update_Display();

                            //保留スロットのアイテムを削除
                            OnHoldSelectSlot.SetItem(null);

                            //保留を空に
                            OnHoldSelectSlot = null;

                            return;
                        }
                    }
                }
            }

            //一時保存用
            EquipmentItem_Base _tempItem = _slot.GetItem();
            //交換
            _slot.SetItem(OnHoldSelectSlot.GetItem());
            OnHoldSelectSlot.SetItem(_tempItem);

            //保留スロットを空に
            OnHoldSelectSlot = null;
            return;
        }
        else
        {
            //保留が空っぽなのでスロットを登録して終了
            OnHoldSelectSlot = _slot;
            return;
        }
    }

    //保留状態にする
    public void SetOnHold(UI_SelectSlot _slot)
    {
        if (OnHoldSelectSlot != null) return;//ほ
    }


    public void SetAcceMode(Accessory _Acce)
    {
        MainSelectUI_LockFalg = true;
        tempAcce = _Acce;
    }
}
