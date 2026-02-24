using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float BulletSpeed = 1.0f;//弾速
    [SerializeField] private float AttackValue = 1.0f;//攻撃力
    [SerializeField] private float BlowPower = 1.0f;//吹き飛ばし力

    [SerializeField] private float DeleteTimer = 1.0f;//削除タイマー
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //移動
        transform.Translate(0.0f, 0.0f, BulletSpeed * Time.deltaTime);

        //タイマー経過で自身を削除
        if (DeleteTimer <= 0.0f)
        {
            Destroy(this.gameObject);
        }

        //タイマー更新
        DeleteTimer = Mathf.Max(0.0f, DeleteTimer - Time.deltaTime);
    }

    protected virtual void IsHit(GameObject _HitObj)
    {
        //ダメージ処理
        if (_HitObj.tag == "Player")//衝突相手がプレイヤー
        {
            Character chara = _HitObj.GetComponent<Character>();
            if (chara != null)
            {
                //死亡状態な終了
                if (chara.GetIsDead()) return;

                //ダメージ計算
                float Def = chara.GetDefensePoint_Result();

                float Damage = AttackValue;//補正は要件等

                //ダメージ適用
                chara.SetDamage(Damage);

                //吹き飛ぶ方向を計算
                Vector3 BlowVec = chara.transform.position - transform.position;//方向を算出
                BlowVec.Normalize();//正規化
                BlowVec *= BlowPower;

                //ノックバックを反映
                chara.SetKnockBack(new Vector2(BlowVec.x, BlowVec.z));

                //弾を破棄
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        IsHit(other.gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        IsHit(collision.gameObject);
    }
}
