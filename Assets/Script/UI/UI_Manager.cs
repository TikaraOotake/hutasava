using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class UI_Manager : MonoBehaviour
{
    [SerializeField] private GameObject DamageDisplayUI_Prefab;//ダメ―ジ表示のUIプレハブ

    [SerializeField] private bool Reward_UI_Flag;//報酬画面の表示フラグ
    [SerializeField] private GameObject CursorObj;//カーソルのオブジェクト

    [SerializeField] private Text MoneyValueText;//残高を表示するテキスト

    [SerializeField] private Canvas canvas;//キャンバス
    [SerializeField] private Camera cameraComp;//カメラのコンポ



    [SerializeField] private List<UI_Base> SelectUI_List;//選択したいUIのリスト
    [SerializeField] private int SelectIndex;
    [SerializeField] private UI_Base SelectingUI;//選択中のUI

    [SerializeField] private float JoyStickInputLength_old;//ジョイスティックの入力値
    void Start()
    {
        //カメラ取得
        if (cameraComp == null) cameraComp = GetComponent<Camera>();

        //キャンバス取得
        if (canvas == null)
        {
            GameObject obj = GameObject.Find("Canvas");
            if (obj != null)
            {
                canvas = obj.GetComponent<Canvas>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //選択状態を次のUIに移行する
        SelectNext();

        //決定入力
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            //UIのイベントを呼び出す------------------
            if (0 <= SelectIndex && SelectUI_List.Count > SelectIndex)//配列内チェック
            {
                if (SelectUI_List[SelectIndex] != null)//ヌルチェック
                {
                    SelectUI_List[SelectIndex].Event();
                }
            }
            //----------------------------------------
        }
    }

    void SelectNext()
    {
        Vector2 inputDir = GetInputDir();
        if (inputDir == Vector2.zero) return;
        if (SelectUI_List[SelectIndex] == null)
        {
            SelectIndex = 0;
            return;
        }
        if (SelectIndex < 0 || SelectIndex >= SelectUI_List.Count)
        {
            SelectIndex = 0;
            return;
        }

        Transform current = SelectUI_List[SelectIndex].transform; // ←番号で取得

        Transform best = null;
        float bestScore = float.MaxValue;
        int bestIndex = SelectIndex;

        for (int i = 0; i < SelectUI_List.Count; i++)
        {
            if (i == SelectIndex) continue;
            if (SelectUI_List[i] == null) continue;

            Vector2 diff = (Vector2)SelectUI_List[i].transform.position - (Vector2)current.position;

            // 入力と逆方向は除外
            if (Vector2.Dot(inputDir, diff.normalized) < 0.2f) continue;

            float angle = Vector2.Angle(inputDir, diff);
            float dist = diff.magnitude;
            float score = angle * 1.0f + dist * 0.2f;

            if (score < bestScore)
            {
                bestScore = score;
                best = SelectUI_List[i].transform;
                bestIndex = i;  // ← インデックスを記録
            }
        }

        if (best != null)
        {
            //移る前のUIの色を戻す
            HighlightUI(SelectUI_List[SelectIndex], false);

            SelectIndex = bestIndex;       // ←選択中インデックスを更新

            //UIを発光
            HighlightUI(SelectUI_List[SelectIndex], true);
        }
    }
    void HighlightUI(UI_Base _ui,bool _LightFlag)
    {
        if (_ui != null)
        {
            _ui.SetSelectingHighlight_UI(_LightFlag);
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

    public void SetUI_Flag(bool _flag)
    {
        Reward_UI_Flag = _flag;
        if (Reward_UI_Flag)
        {
            Time.timeScale = 0.0f;//時間停止
        }
        else
        {
            Time.timeScale = 1.0f;//時間再生
        }
    }
    public void SetCursorObj(GameObject _Cursor)
    {
        CursorObj = _Cursor;
    }
    public GameObject GetCursorObj()
    {
        return CursorObj;
    }
    public void SetMoneyValue_UI(int _value)
    {
        if (MoneyValueText != null)
        {
            MoneyValueText.text = _value.ToString() + "G";
        }
    }
    public void GenerateDamageDisplayUI(int _Damage, Vector3 _Pos)
    {
        Debug.Log("ダメージ表記を生成します");

        if (DamageDisplayUI_Prefab != null && canvas != null && cameraComp)
        {
            GameObject damageObj = Instantiate(
                DamageDisplayUI_Prefab,
                canvas.transform
            );

            UI_DamageDisplay ui = damageObj.GetComponent<UI_DamageDisplay>();
            if (ui != null)
            {
                ui.SetDisplayDamage(_Damage);
                ui.Setup(_Pos,canvas,cameraComp);
            }
        }
        else
        {
            Debug.Log("必要な変数がありませんでした");
        }
    }
    
}
