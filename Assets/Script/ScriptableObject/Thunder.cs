using UnityEngine;

public class Thunder : Weapon
{

    private int AttackCount = 0;
    [SerializeField]
    private float ComboCooltime_Ratio = 1.0f;
    private float ComboCooltimer;

    [SerializeField] private Vector2 AttackRadius = new Vector2(0.0f, 1.0f);//攻撃半径
    public override void Update_Item(GameObject _CallObject)//引き数：_CallObject　呼び出し元のオブジェクト
    {
        //タイマーが0なら攻撃
        if (AttackCooltimer <= 0.0f)
        {
            if (ComboCooltimer <= 0.0f)
            {
                if (BulletPrefab != null)
                {
                    //プレイヤーの座標取得
                    Vector3 pos = _CallObject.transform.position;

                    //ランダムな方向の正規化ベクトル
                    float angle = Random.Range(0f, Mathf.PI * 2f);
                    Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

                    //正規化ベクトルを一定範囲で拡大
                    dir *= Random.Range(AttackRadius.x, AttackRadius.y);

                    //弾生成
                    GameObject newBullet = GenerateBullet(_CallObject, BulletPrefab);

                    //座標設定
                    newBullet.transform.position = pos + new Vector3(dir.x, 0.0f, dir.y);
                }

                //3回攻撃したら攻撃回数リセット
                if (AttackCount >= 3)
                {
                    AttackCount = 0;

                    //タイマー再設定
                    AttackCooltimer = AttackCooltime;
                }

                //タイマー再設定
                ComboCooltimer = AttackCooltime * ComboCooltime_Ratio;
            }
        }

        //タイマー更新
        AttackCooltimer = Mathf.Max(0.0f, AttackCooltimer - Time.deltaTime);
        ComboCooltimer = Mathf.Max(0.0f, ComboCooltimer - Time.deltaTime);
    }
}
