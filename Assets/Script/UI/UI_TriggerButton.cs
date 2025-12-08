using UnityEngine;
using UnityEngine.UI;
using System;

public class UI_TriggerButton : MonoBehaviour
{
    [SerializeField] private bool ActiveFlag;
    // デリゲート宣言
    //public delegate void CallFunction();
    private Action onCall;          // インスタンス（他スクリプトから代入する）

    private void Start()
    {
        //SetFunction(Test);
    }

    private void Update()
    {
        if (ActiveFlag)
        {
            onCall?.Invoke();  // ← 外部の関数を呼ぶ
        }
        ActiveFlag = false;
    }
    public void SetActiveFlag(bool _falg)
    {
        ActiveFlag = _falg;
    }

    public void SetFunction(Action func)
    {
        onCall = func;
    }
    private void Test()
    {

    }
}
