using System;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class UI_Base : MonoBehaviour
{
    public event Action<int> OnSelected;

    private Color BaseColor;

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
        OnSelected?.Invoke(-1);
    }

    public virtual void SetSelectingHighlight_UI(bool _flag)
    {
        IsSelectiveFalg = _flag;
        SetHighlight_UI(_flag);
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
}
