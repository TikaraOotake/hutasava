using UnityEngine;

[CreateAssetMenu(fileName = "CrossBullet", menuName = "ScriptableObjects/CreateWeapon_CrossBullet")]
public class CrossBullet : Weapon
{
    //アイテムの更新処理
    public override void Update_Item(GameObject _CallObject)//引き数：_CallObject　呼び出し元のオブジェクト
    {
        //タイマーが0なら攻撃
        if (AttackCooltimer == 0.0f)
        {
            if(BulletPrefab!=null)
            {
                for (int i = 0; i < 4; ++i)
                {
                    GameObject newBullet = Instantiate(BulletPrefab, _CallObject.transform.position, Quaternion.identity);
                    newBullet.transform.eulerAngles = new Vector3(0.0f, 90 * i + _CallObject.transform.eulerAngles.y, 0.0f);
                }
            }

            //タイマー再設定
            AttackCooltimer = AttackCooltime;
        }

        //タイマー更新
        AttackCooltimer = Mathf.Max(0.0f, AttackCooltimer - Time.deltaTime);
    }
}
