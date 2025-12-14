using UnityEngine;
using UnityEngine.UI;

public class UI_DamageDisplay : MonoBehaviour
{
    [SerializeField] private int Damage;

    [SerializeField]
    private float DisplayTime = 1.0f;//表示時間
    private float DisplayTimer;//表示タイマ

    //座標計算に必要な変数
    [SerializeField] private Vector3 worldPos;//ワールド座標
    [SerializeField] private Canvas canvasComp;
    [SerializeField] private Camera cameraComp;

    [SerializeField] private float RandomShiftLength_WorldPos = 1.0f;//ワールド座標をRandomにずらす距離

    [SerializeField] 
    private float TargetShiftHeight = 1.0f;//ずらす高さ目標
    private float ShiftHeight = 0.0f;//ずらす高さ
    [SerializeField] private float HeightLerpLevel;//高さをどれだけ滑らかにずらすか

    [SerializeField] private float FadeSpeed = 1.0f;//フェード速度
    [SerializeField] private float FadeRate = 0.0f;//フェード率

    Vector3 BaseScale;

    [SerializeField] private Text text;//ダメージを表示するテキスト
    [SerializeField] private Outline outline;//アウトライン
    [SerializeField] private RectTransform rectTransform;//UIのトランスフォーム
    void Start()
    {
        //取得
        if (text == null) text = GetComponent<Text>();
        if (outline == null) outline = GetComponent<Outline>();
        if (rectTransform == null) rectTransform = GetComponent<RectTransform>();

        //タイマー初期化
        DisplayTimer = DisplayTime;

        //基準サイズを取得
        if (rectTransform != null) BaseScale = rectTransform.localScale;

        //ワールド座標をランダムにずらす
        worldPos.x += Random.Range(-RandomShiftLength_WorldPos, RandomShiftLength_WorldPos);
        worldPos.y += Random.Range(-RandomShiftLength_WorldPos, RandomShiftLength_WorldPos);
        worldPos.z += Random.Range(-RandomShiftLength_WorldPos, RandomShiftLength_WorldPos);

        //
        if (text != null) text.text = Damage.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        Update_Pos();

        if (DisplayTimer > 0.0f)
        {
            FadeRate = Mathf.Min(1.0f, FadeRate + Time.deltaTime * FadeSpeed * 2.0f);
        }
        else
        {
            FadeRate = Mathf.Max(0.0f, FadeRate - Time.deltaTime * FadeSpeed);
        }

        //テキストのカラー変更
        if (text != null)
        {
            Color color = text.color;
            color.a = FadeRate;
            text.color = color;
        }
        if (outline != null)
        {
            Color color = outline.effectColor;
            color.a = FadeRate;
            outline.effectColor = color;
        }

        //大きさを変更
        if (rectTransform != null)
        {
            rectTransform.localScale = BaseScale * FadeRate;
        }

        //表示する状態じゃなくなったら削除
        if (FadeRate <= 0.0f && DisplayTimer <= 0.0f)
        {
            Destroy(gameObject);
        }

        //タイマー更新
        DisplayTimer = Mathf.Max(0.0f, DisplayTimer - Time.deltaTime);
    }

    public void SetDisplayDamage(int _Damage)
    {
        Damage = _Damage;
    }
    public void Setup(Vector3 _worldPos, Canvas _canvasComp,Camera _cameraComp)
    {
        worldPos = _worldPos;
        canvasComp = _canvasComp;
        cameraComp = _cameraComp;
    }
    //ワールド座標からキャンバスの座標に変換する
    private static Vector2 WorldToCanvasPosition(
    Canvas canvas,
    Camera camera,
    Vector3 worldPosition
)
    {
        //必要な変数があるかチェック
        if (canvas == null || camera == null) return Vector2.zero;

        Vector3 screenPos = camera.WorldToScreenPoint(worldPosition);

        RectTransform canvasRect = canvas.transform as RectTransform;

        Vector2 canvasPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPos,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : camera,
            out canvasPos
        );

        return canvasPos;
    }
    private void Update_Pos()
    {
        //ワールド座標をキャンバス座標に変換
        Vector2 pos = WorldToCanvasPosition(canvasComp, cameraComp, worldPos);

        //ずらす高さを計算
        //滑らかに目標高さに補正
        ShiftHeight = Mathf.Lerp(ShiftHeight, TargetShiftHeight, 1 - Mathf.Exp(-HeightLerpLevel * Time.deltaTime));

        //高さをずらす
        pos.y += ShiftHeight;

        //座標を適用
        RectTransform rect = GetComponent<RectTransform>();
        rect.anchoredPosition = pos;
    }
}
