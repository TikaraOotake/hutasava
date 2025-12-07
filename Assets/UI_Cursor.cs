using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Cursor : MonoBehaviour
{
    private RectTransform rectTransform;
    private Canvas canvas;

    public Canvas targetCanvas;
    GraphicRaycaster raycaster;
    PointerEventData pointerData;
    EventSystem eventSystem;



    void Start()
    {
        //UIマネージャーに自身を登録
        GameManager.Instance.SetCursorObj(this.gameObject);

        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        GameObject Canvas = gameObject.transform.root.gameObject;
        if (Canvas) targetCanvas = Canvas.GetComponent<Canvas>();
        if (targetCanvas) raycaster = targetCanvas.GetComponent<GraphicRaycaster>();
        eventSystem = EventSystem.current;
    }
    void Update()
    {
        Vector2 mousePos;
        // マウスのスクリーン座標をCanvasのローカル座標に変換
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Input.mousePosition,
            canvas.worldCamera,
            out mousePos
        );

        // UIの位置をマウス位置に設定
        rectTransform.anchoredPosition = mousePos;


        if (Input.GetMouseButtonDown(0))
        {
            Image image = GetTopImage(Input.mousePosition);
            if (image != null)
            {
                Debug.Log(image.gameObject + "と重なっています");

                UI_TriggerButton _TriggerButton = image.GetComponent<UI_TriggerButton>();
                if (_TriggerButton != null)
                {
                    _TriggerButton.SetActiveFlag(true);
                }
            }
            else
            {
                Debug.Log("なにも重なっていません");
            }
        }
    }

    public Image GetTopImage(Vector2 screenPos)
    {
        var images = GetImagesAtPoint(screenPos);

        foreach (var img in images)
        {
            if (img.raycastTarget == true)
                return img;
        }

        return null;
    }
    /// <summary>
    /// canvas 内の座標（screenPos）に重なっている Image をすべて取得
    /// </summary>
    public List<Image> GetImagesAtPoint(Vector2 screenPos)
    {
        pointerData = new PointerEventData(eventSystem);
        pointerData.position = screenPos;

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerData, results);

        List<Image> images = new List<Image>();

        foreach (var result in results)
        {
            Image img = result.gameObject.GetComponent<Image>();
            if (img != null)
            {
                images.Add(img);
            }
        }

        return images;
    }
}
