using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CreateAssetMenu(fileName = "ChaseBullet", menuName = "ScriptableObjects/CreateWeapon_ChaseBullet")]
public class ChaseBullet : Weapon
{
    //アイテムの更新処理
    public override void Update_Item(GameObject _CallObject)//引き数：_CallObject　呼び出し元のオブジェクト
    {
        //タイマーが0なら攻撃
        if (AttackCooltimer == 0.0f)
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
            }

            //タイマー再設定
            AttackCooltimer = AttackCooltime;
        }

        //タイマー更新
        AttackCooltimer = Mathf.Max(0.0f, AttackCooltimer - Time.deltaTime);
    }
}
