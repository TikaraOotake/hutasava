using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PairChakramBullet : PlayerBullet
{
    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;

    [SerializeField] private bool FlipFlag;
    private float sign = 1.0f;//点Pの移動符号
    private float sign_old;//符号監視変数

    [SerializeField] private string ShotSE;

    [SerializeField] private float RotateSpeed;//回転速度


    private float Ratio;//二点間の割合座標

    void Start()
    {
        //符号監視変数更新
        sign_old = sign;
    }

    // Update is called once per frame
    void Update()
    {

        GameObject TargetObj = null;
        if (FlipFlag == true)
        {
            TargetObj = player1;
        }
        else
        {
            TargetObj = player2;
        }

        //ヌルチェック
        if (player1 == null || player2 == null) return;

        // ターゲットのXZ座標のみを使う（Yは自分と同じにする）
        Vector3 targetPos = TargetObj.transform.position;
        targetPos.y = transform.position.y;

        //任意の方向を向き続ける
        //transform.LookAt(targetPos);

        //移動
        //transform.Translate(new Vector3(0.0f, 0.0f, 1.0f) * BulletSpeed * Time.deltaTime);

        //座標取得
        Vector3 pos1 = player1.transform.position;
        Vector3 pos2 = player2.transform.position;
        pos1.y = pos2.y = 0.0f;//高さを無視

        //距離を算出
        float Length = Vector3.Distance(pos1, pos2);
        if (Length <= BulletSpeed * Time.deltaTime)
        {
            //FlipFlag = !FlipFlag;
        }

        // 点Pの移動
        if (Length != 0.0f)
        {
            Ratio += sign * BulletSpeed * Time.deltaTime / Length;
        }

        // 先に反転判定
        if (Ratio >= 1.0f)
        {
            Ratio = 1.0f;
            sign = -1.0f;
        }
        else if (Ratio <= 0.0f)
        {
            Ratio = 0.0f;
            sign = 1.0f;
        }

        //ヌルチェック
        if (player1 == null || player2 == null) return;

        //座標計算
        Vector3 pos = Vector3.Lerp(player1.transform.position, player2.transform.position, Ratio);
        pos.y = TargetObj.transform.position.y;//Y座標を無視

        //回転する
        Vector3 angle = transform.eulerAngles;
        angle.y += RotateSpeed * BulletSpeed * Time.deltaTime;
        transform.eulerAngles = angle;

        //代入
        transform.position = pos;


        //符号の変化を見る
        if (sign != sign_old)
        {
            //音を再生
            SoundManager.instance.PlaySE(ShotSE);
        }

        //符号監視変数更新
        sign_old = sign;
    }

    public void SetPlayer(GameObject _player1, GameObject _player2)
    {
        player1 = _player1;
        player2 = _player2;
    }
}
