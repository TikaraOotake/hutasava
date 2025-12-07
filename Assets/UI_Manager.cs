using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] private bool Reward_UI_Flag;//報酬画面の表示フラグ
    [SerializeField] private GameObject CursorObj;//カーソルのオブジェクト
    void Start()
    {
        
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
}
