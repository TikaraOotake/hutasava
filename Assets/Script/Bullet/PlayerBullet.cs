using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] protected GameObject Player;//この弾を撃つプレイヤー
    [SerializeField] protected Weapon weapon;//この弾を撃つ武器データ

    [SerializeField] private float BulletSpeed = 1.0f;
    [SerializeField] private float BulletAtk = 1.0f;

    [SerializeField] private Vector3 BulletMoveVec;

    [SerializeField] private float DestroyTimer = 1.0f;//削除タイマー

    [SerializeField] private float hitInterval = 0.5f; // 連続Hit間隔
    Dictionary<GameObject, float> hitCooldown = new Dictionary<GameObject, float>();

    float cleanupTimer;

    [SerializeField] private bool isPerforate;//弾貫通フラグ

    [SerializeField] private List<GameObject> HittedObjList;

    public void SetPlayer(GameObject _Player)
    {
        Player = _Player;
    }
    public void SetWeapon(Weapon _weapon)
    {
        weapon = _weapon;
    }

    void Update()
    {
        transform.Translate(new Vector3(0.0f, 0.0f, BulletSpeed) * Time.deltaTime);

        //タイマー更新
        DestroyTimer = Mathf.Max(0.0f, DestroyTimer - Time.deltaTime);

        if (DestroyTimer <= 0.0f)
        {
            Destroy(this.gameObject);
        }

        //弾共通の更新処理------------
        Update_bullet();
    }

    //弾共通の更新処理
    protected void Update_bullet()
    {
        Cleanup();
    }

    public void SetAtkValue(float _Atk)
    {
        BulletAtk = _Atk;
    }

    //既に当たったオブジェクトかどうかチェック
    private bool CheckHittedObj(GameObject _obj)
    {
        bool Result = false;
        for (int i = 0; i < HittedObjList.Count; ++i)
        {
            if (_obj == HittedObjList[i])
            {
                Result = true;
                break;
            }
        }

        return Result;
    }

    private bool TryHit(GameObject enemy)
    {
        float now = Time.time;

        if (hitCooldown.TryGetValue(enemy, out float nextHitTime))
        {
            if (now < nextHitTime) return false; // まだ再Hit不可
        }

        // ダメージ処理
        //DealDamage(enemy);

        // 次にHit可能な時刻を更新
        hitCooldown[enemy] = now + hitInterval;

        //ヒット可能
        return true;
    }

    //連続hit管理リストの掃除
    protected void Cleanup()
    {
        cleanupTimer += Time.deltaTime;
        if (cleanupTimer < 1.0f) return;

        cleanupTimer = 0f;

        float now = Time.time;

        var removeList = new List<GameObject>();

        foreach (var pair in hitCooldown)
        {
            if (now >= pair.Value)
                removeList.Add(pair.Key);
        }

        foreach (var key in removeList)
        {
            hitCooldown.Remove(key);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        //仮ダメージ処理
        if (other.tag == "Enemy")//衝突相手がエネミー
        {
            //既に当たっていないかチェック
            //if (!CheckHittedObj(other.gameObject))
            if (TryHit(other.gameObject))
            {
                Character chara = other.GetComponent<Character>();
                if (chara != null)
                {
                    //ダメージ計算
                    float Def = chara.GetDefensePoint_Result();

                    float Damage = BulletAtk;//補正は要件等

                    //ダメージ適用
                    chara.SetDamage(Damage);

                    //吹き飛ぶ方向を計算
                    Vector3 BlowVec = chara.transform.position - transform.position;//方向を算出
                    BlowVec.Normalize();//正規化
                    BlowVec *= 0.5f;//いい感じに補正

                    //ノックバックを反映
                    chara.SetKnockBack(new Vector2(BlowVec.x, BlowVec.z));

                    //当たったオブジェクトを記録
                    HittedObjList.Add(other.gameObject);

                    //自身を削除
                    if (isPerforate == false) Destroy(this.gameObject);
                }
            }
        }
    }
}
