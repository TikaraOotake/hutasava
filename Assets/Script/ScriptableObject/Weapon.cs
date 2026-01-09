using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/CreateWeapon")]
public class Weapon : EquipmentItem_Base
{
    [SerializeField] protected GameObject BulletPrefab;//弾のプレハブ

    [SerializeField]
    protected float AttackCooltime = 1.0f;//再使用時間
    protected float AttackCooltimer;//再使用時間計測

    [SerializeField]
    protected float WeaponAttackPoint_Base = 1;//武器基礎攻撃力
    [SerializeField]
    protected int WeaponLevel = 0;

    //初期化
    public override void Init()
    {
        return;
    }

    //アイテムの更新処理
    public virtual void Update_Item(GameObject _CallObject)//引き数：_CallObject　呼び出し元のオブジェクト
    {
        //タイマーが0なら攻撃
        if (AttackCooltimer == 0.0f)
        {
            //タイマー再設定
            AttackCooltimer = AttackCooltime;
        }

        //タイマー更新
        AttackCooltimer = Mathf.Max(0.0f, AttackCooltimer - Time.deltaTime);
    }

    //弾の攻撃力代入関数
    protected void SetBulletAtkValue(GameObject _bulletObj, float _AtkValue)
    {
        if (_bulletObj)
        {
            Bullet_01 bullet = _bulletObj.GetComponent<Bullet_01>();
            if (bullet)
            {
                bullet.SetAtkValue(_AtkValue);
            }
        }
    }

    public int GetWeaponLevel()
    {
        return WeaponLevel;
    }
    public void SetWeaponLevel(int _WeaponLevel)
    {
        WeaponLevel = _WeaponLevel;
    }

    //弾の生成と情報の受け渡しをある程度自動で行う関数
    protected GameObject GenerateBullet(GameObject _CallObject,GameObject _BulletPrefab)
    {
        //生成
        GameObject bullet = Instantiate(_BulletPrefab, _CallObject.transform.position, Quaternion.identity);

        PlayerBullet playerBullet = bullet.GetComponent<PlayerBullet>();
        if (playerBullet != null)
        {
            playerBullet.SetPlayer(_CallObject);//プレイヤー設定
            playerBullet.SetWeapon(this);//武器データ設定

            //攻撃力適用
            float PlayerAtk = 1;
            PlayerController_3d player = _CallObject.GetComponent<PlayerController_3d>();//プレイヤースクリプト取得
            if (player) PlayerAtk = player.GetAttackPoint_Result();//プレイヤー攻撃力取得
            SetBulletAtkValue(bullet, PlayerAtk * WeaponAttackPoint_Base);//攻撃力適用
        }

        return bullet;
    }
}
