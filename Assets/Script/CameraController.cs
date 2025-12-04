using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private float CameraSmoothRotateRate = 0.5f;//滑らか回転割合
    [SerializeField] private float CameraSmoothMoveRate = 0.5f;//滑らか移動割合
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
        Vector2 Player1Pos = Player1.transform.position;
        Vector2 Player2Pos = Player2.transform.position;

        //角度を調べる
        float angle = Mathf.Atan2(Player2Pos.y - Player1Pos.y, Player2Pos.x - Player1Pos.x) * Mathf.Rad2Deg;

        //座標を調べる
        Vector2 midPos = Player1Pos + (Player2Pos - Player1Pos) * 0.5f;//二点間の中心を調べる


        //滑らかに遷移するように補正
        Vector2 currentPos = transform.position;
        Vector2 ResultPos = Vector2.Lerp(currentPos, midPos, 1 - Mathf.Exp(-1.0f * Time.deltaTime));//座標

        float currentAngle = transform.eulerAngles.z;
        float ResultAngle = Mathf.LerpAngle(currentAngle, angle, 1 - Mathf.Exp(-CameraSmoothRotateRate * Time.deltaTime));//角度

        //値を適用
        transform.position = new Vector3(ResultPos.x, ResultPos.y, transform.position.z);
        transform.eulerAngles = new Vector3(0.0f, 0.0f, ResultAngle);



        //A = Mathf.MoveTowards(A, B, 0.5f * Time.deltaTime);
    }
}
