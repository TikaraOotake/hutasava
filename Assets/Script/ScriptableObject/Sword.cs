using UnityEngine;

[CreateAssetMenu(fileName = "Sword", menuName = "ScriptableObjects/CreateWeapon_Sword")]
public class NewMonoBehaviourScript : Weapon
{
    private int AttackCount = 0;

    [SerializeField]
    private float ComboCooltime_Ratio = 1.0f;
    private float ComboCooltimer;
    public override void Update_Item(GameObject _CallObject)//引き数：_CallObject　呼び出し元のオブジェクト
    {
        //タイマーが0なら攻撃
        if (AttackCooltimer <= 0.0f)
        {
            if (ComboCooltimer <= 0.0f)
            {
                if (BulletPrefab != null)
                {
                    //呼び出し者と最も近いエネミーを取得
                    GameObject Enemy = GameManager.Instance.GetNearestEnemy(_CallObject.transform.position);

                    float angle = 0.0f;//角度

                    if (Enemy != null)
                    {
                        //方向を算出
                        Vector3 direction = (Enemy.transform.position - _CallObject.transform.position).normalized;

                        //角度に変換
                        angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
                    }
                    else
                    {
                        angle = _CallObject.transform.eulerAngles.y;
                    }

                    GameObject newBullet = Instantiate(BulletPrefab, _CallObject.transform.position, Quaternion.identity);
                    newBullet.transform.eulerAngles = new Vector3(0.0f, 90 - angle, 0.0f);

                    //攻撃力適用
                    float PlayerAtk = 1;
                    PlayerController_3d player = _CallObject.GetComponent<PlayerController_3d>();//プレイヤースクリプト取得
                    if (player) PlayerAtk = player.GetAttackPoint_Result();//プレイヤー攻撃力取得
                    SetBulletAtkValue(newBullet, PlayerAtk * WeaponAttackPoint_Base);//攻撃力適用

                    //3回目は攻撃を大きくする
                    if (AttackCount >= 2)
                    {
                        Vector3 Scale = newBullet.transform.localScale;
                        newBullet.transform.localScale = Scale * 1.5f;
                    }
                }

                //攻撃回数を加算
                ++AttackCount;

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
