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
