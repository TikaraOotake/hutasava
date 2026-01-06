using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI_SelectWindow : MonoBehaviour
{
    [SerializeField] private List<UI_Base> SelectButtonList;//選択肢ボタンのリスト

    private UI_Manager ui_Manager;

    bool initialized = false;//初期化したかどうかのフラグ

    private void Start()
    {
        Init();
    }
    private void Init()
    {
        if (initialized) return;
        initialized = true;

        ui_Manager = GameManager.Instance.GetUI_Manager();

        for (int i = 0; i < SelectButtonList.Count; ++i)
        {
            SelectButtonList[i].OnSelected += TestFunction;
        }
    }

    private void TestFunction(int i)
    {
        //Debug.Log("テスト　引数：" + i);
    }

    //関数の設定
    public void SetAction(int _index, Action<int> _Action, int _id)
    {
        if (_index >= 0 && _index < SelectButtonList.Count)//配列外チェック
        {
            if (SelectButtonList[_index] != null)//ヌルチェック
            {
                SelectButtonList[_index].SetID(_id);//IDセット
                SelectButtonList[_index].OnSelected += _Action;//イベントセット
                Debug.Log("関数を設定しました");
                return;//終了
            }
        }
        Debug.Log("選択後の関数の設定に失敗しました");
    }

    //選択ウィンドウの有効化設定
    public void SetActiveWindow(bool _falg)
    {
        Init();

        if (_falg)
        {
            //表示
            this.gameObject.SetActive(true);

            //全ての選択肢を選択リストから除外
            ui_Manager.RemoveSelectSlot_all();

            //選択したいものを選択リストに追加
            ui_Manager.SetSelectSlot_isSelective(SelectButtonList);
        }
        else
        {
            //非表示
            this.gameObject.SetActive(false);

            //選択から除外したいものをリストから外す
            ui_Manager.RemoveSelectSlot(SelectButtonList);
        }
    }
}
