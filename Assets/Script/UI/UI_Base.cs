using System;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class UI_Base : MonoBehaviour
{
    public event Action<int> OnSelected;

    private Color BaseColor;

    private int ID = 0;//どこを担当しているか識別するためのID

    [SerializeField] protected Vector3 UI_BaseScale;//UIの基準の大きさ
    [SerializeField] protected float SelectUpScaleRate = 1.2f;//選択時に拡大させる割合

    [SerializeField] private Image HighlightImage;//選択時に発光させるイメージ
    [SerializeField] protected bool HighlightFalg;//発光フラグ

    [SerializeField] protected bool IsSelectiveFalg;//選択状態フラグ
    public void Awake()
    {
        Image image = GetComponent<Image>();
        if (image != null)
        {
            BaseColor = image.color;
        }

        //基準サイズを記録
        UI_BaseScale = transform.localScale;

        SetSelectingHighlight_UI(false);
    }

    //イベントを呼び出す
    public virtual void Event()
    {
        InvokeSelected();
        return;
    }
    protected void InvokeSelected()
    {
        //Debug.Log("イベントを呼び出します");
        OnSelected?.Invoke(ID);
    }

    public virtual void SetSelectingHighlight_UI(bool _flag)
    {
        IsSelectiveFalg = _flag;
        SetHighlight_UI(_flag);

        //選択中にUIを拡大させる
        if (IsSelectiveFalg)
        {
            transform.localScale = UI_BaseScale * SelectUpScaleRate;//大きさを拡大
        }
        else
        {
            transform.localScale = UI_BaseScale;//大きさを通常に
        }
    }
    public virtual void SetHighlight_UI(bool _flag)
    {
        HighlightFalg = _flag;
        HighlightUI(HighlightFalg);
    }
    protected void HighlightUI(bool _falg)
    {
        Image image = GetComponent<Image>();
        if (image != null)
        {
            if (HighlightFalg)
            {
                //黄色に発光
                Color color = Color.yellow;
                color.a = BaseColor.a;
                image.color = color;
            }
            else
            {
                //通常色
                image.color = BaseColor;
            }
        }
    }

    public void  SetID(int _id)
    {
        ID= _id;
    }
}
