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
    public void Awake()
    {
        //SetSelectingHighlight_UI(false);
    }
    public void Start()
    {
        Image image = GetComponent<Image>();
        if (image != null)
        {
            BaseColor = image.color;
        }
    }

    //イベントを呼び出す
    public virtual void Event()
    {
        //OnSelected?.Invoke(itemId);
        return;
    }

    public virtual void SetSelectingHighlight_UI(bool _flag)
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
