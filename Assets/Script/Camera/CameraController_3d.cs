using UnityEngine;

public class CameraController_3d : MonoBehaviour
{
    [SerializeField] private float CameraSmoothRotateRate = 0.5f;//滑らか回転割合
    [SerializeField] private float CameraSmoothMoveRate = 0.5f;//滑らか移動割合

    [SerializeField] private float CameraLength = 1.0f;//カメラ距離
    [SerializeField] private float CameraLengthRatio = 1.0f;//カメラ距離倍率
    [SerializeField] private float CameraZoomSmoothRate = 1.0f;//カメラズーム滑らか率
    [SerializeField] private Vector2 CameraZoomMinMax;//最大値最小値


    [SerializeField] private Vector3 GazePoint;//視点

    [SerializeField] private float CameraRotateLockLength = 1.0f;//カメラ回転を固定する距離
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera();
    }
    //プレイヤーに合わせてカメラを動かす
    private void MoveCamera()
    {
        //プレイヤー取得
        GameObject Player1 = GameManager.Instance.GetPlayer(1);
        GameObject Player2 = GameManager.Instance.GetPlayer(2);

        //取得の成功を確認
        if (Player1 == null || Player2 == null) return;//失敗している場合は終了

        //座標を取得
        Vector2 Player1Pos = new Vector2(Player1.transform.position.x, Player1.transform.position.z);
        Vector2 Player2Pos = new Vector2(Player2.transform.position.x, Player2.transform.position.z);

        //距離を算出
        float PlayerLength = Vector3.Distance(Player1.transform.position, Player2.transform.position);

        //角度を調べる
        float angle = Mathf.Atan2(Player2Pos.y - Player1Pos.y, Player2Pos.x - Player1Pos.x) * Mathf.Rad2Deg;

        //座標を調べる
        Vector2 midPos = Player1Pos + (Player2Pos - Player1Pos) * 0.5f;//二点間の中心を調べる
        float midHeight = Player1.transform.position.y + (Player2.transform.position.y - Player1.transform.position.y) * 0.5f;//二点間の高さ

        //滑らかに遷移するように補正
        Vector2 currentPos = new Vector2(GazePoint.x, GazePoint.z);
        Vector2 ResultPos = Vector2.Lerp(currentPos, midPos, 1 - Mathf.Exp(-1.0f * Time.deltaTime));//座標

        float currentAngle = -transform.eulerAngles.y;
        float ResultAngle = Mathf.LerpAngle(currentAngle, angle, 1 - Mathf.Exp(-CameraSmoothRotateRate * PlayerLength * Time.deltaTime));//角度

        CameraLength = Mathf.Lerp(CameraLength, PlayerLength, 1 - Mathf.Exp(-CameraZoomSmoothRate * Time.deltaTime));//カメラ距離

        //カメラ距離が限界値を変えないように補正
        float ResultCameraLength = CameraLength * CameraLengthRatio;
        ResultCameraLength = Mathf.Max(ResultCameraLength, Mathf.Min(CameraZoomMinMax.x, CameraZoomMinMax.y));
        ResultCameraLength = Mathf.Min(ResultCameraLength, Mathf.Max(CameraZoomMinMax.x, CameraZoomMinMax.y));

        //値を適用
        GazePoint = new Vector3(ResultPos.x, midHeight, ResultPos.y);

        //距離
        if (CameraRotateLockLength < PlayerLength)
        {
            //回転を適用
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, -ResultAngle, transform.eulerAngles.z);
        }


        //距離を離す
        Vector3 dir = Quaternion.Euler(transform.eulerAngles) * Vector3.forward;

        //座標を適用
        transform.position = GazePoint - (dir * ResultCameraLength);


        //A = Mathf.MoveTowards(A, B, 0.5f * Time.deltaTime);
    }
}
