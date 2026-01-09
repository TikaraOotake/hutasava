using UnityEngine;


[CreateAssetMenu(fileName = "Chakram", menuName = "ScriptableObjects/CreateWeapon_Chakram")]
public class Chakram : Weapon
{
    [SerializeField] GameObject bullet;//生成後の弾

    public override void Init()
    {
        //弾が生成されていたら削除
        if (bullet != null)
        {
            Destroy(bullet);
            bullet = null;
        }
    }
    

    //アイテムの更新処理
    public override void Update_Item(GameObject _CallObject)//引き数：_CallObject　呼び出し元のオブジェクト
    {
        //弾がまだ生成されていなかったら生成
        if (bullet == null && BulletPrefab != null)
        {
            //生成
            bullet = Instantiate(BulletPrefab, _CallObject.transform.position, Quaternion.identity);

            PlayerBullet playerBullet = bullet.GetComponent<PlayerBullet>();
            if (playerBullet != null)
            {
                playerBullet.SetPlayer(_CallObject);//プレイヤー設定
                playerBullet.SetWeapon(this);//武器データ設定
            }
        }
        else
        {

        }
    }
}
