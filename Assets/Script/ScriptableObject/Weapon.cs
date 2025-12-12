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
}
