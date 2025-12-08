using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemChoice : MonoBehaviour
{
    [SerializeField] private GameObject GrabObj;//掴まれているオブジェクト
    [SerializeField] private Vector2 BasePos;//基本位置

    private void Start()
    {


        //UI_TriggerButton triggerButton=GetComponent<UI_TriggerButton>();
        //if(triggerButton)
        //{
        //    triggerButton.SetFunction(SetCursor);
        //}
    }
    private void Update()
    {
        if (GrabObj != null)
        {

        }
        else
        {

        }
    }

    private void SetCursor()
    {
        GrabObj = GameManager.Instance.GetCursorObj();
    }
    private void SetCursor(GameObject _Cursor)
    {
        GrabObj = _Cursor;
    }
}
