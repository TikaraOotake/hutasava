using UnityEngine;

public class Skeleton : Enemy_3d
{
    //弾のPrefab
    [SerializeField] private GameObject EnemyBulletPrefab;

    //発射距離
    [SerializeField] private float ShootingDistance;
    [SerializeField] private float AttackInterval = 1.0f;//攻撃間隔
    [SerializeField] private float AttackTimer;//攻撃タイマー

    void Start()
    {
        //攻撃タイマーリセット
        AttackTimer = AttackInterval;
    }

    void Update()
    {
        //移動先を探す
        SearchChasePlayer();

        //距離を計算
        Vector3 pos1 = transform.position;
        Vector3 pos2 = TargetPoint;
        pos1.y = pos2.y = 0.0f;//高さを揃える
        float Distance = Vector3.Distance(pos1, pos2);

        //発射距離か計測
        if (Distance <= ShootingDistance)
        {
            //攻撃処理

            //発射
            if (AttackTimer <= 0.0f)
            {
                //
                GenerateBullet(EnemyBulletPrefab, pos2 - pos1);

                //攻撃タイマーリセット
                AttackTimer = AttackInterval;
            }

            //タイマー更新
            AttackTimer = Mathf.Max(0.0f, AttackTimer - Time.deltaTime);
        }
        else
        {
            //移動処理

            //攻撃タイマーリセット
            AttackTimer = AttackInterval* 0.5f;//半分くらいから始める

            //移動
            Move();
        }

        Update_Blow();
    }

    private void GenerateBullet(GameObject _obj, Vector3 _vec)
    {
        //ヌルチェック
        if (_obj != null)
        {
            //正規化
            _vec.Normalize();

            float angle = Mathf.Atan2(_vec.z, _vec.x) * Mathf.Rad2Deg;

            GameObject bullet = Instantiate(_obj);
            Vector3 BulletAngle = bullet.transform.eulerAngles;
            BulletAngle.y = -angle + 90.0f;
            bullet.transform.eulerAngles = BulletAngle;//角度を代入
            bullet.transform.position = transform.position;//座標を代入
        }
    }
}
