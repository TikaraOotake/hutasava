using UnityEngine;

[CreateAssetMenu(fileName = "PairChakram", menuName = "ScriptableObjects/CreateWeapon_ PairChakram")]
public class PairChakram : Weapon
{
    [SerializeField]
    private GameObject FiredBullet = null;//発射した弾を記録しておく変数

    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;

    private bool InitFlag = false;
    public override void Update_Item(GameObject _CallObject)
    {
        //初期化
        Init();

        //発射していないなら弾を生成
        if (FiredBullet == null && BulletPrefab != null)
        {
            FiredBullet = Instantiate(BulletPrefab);//弾生成
            BulletStatusSetting();//Status設定
        }
    }

    public void BulletStatusSetting()
    {
        //Statusの変更が掛かるたびに呼び出す関数

        if (FiredBullet == null) return;//ヌルチェック

        PairChakramBullet pairChakramBullet = FiredBullet.GetComponent<PairChakramBullet>();
        if (pairChakramBullet != null)
        {
            //プレイヤー設定
            pairChakramBullet.SetPlayer(player1, player2);

            Debug.Log("ペアチャクラムにプレイヤーを設定しました");
        }
    }
    public override void Init()
    {
        //プレイヤー取得に問題がある場合は初期化を必要と判断
        if (player1 == null || player2 == null) InitFlag = false;//フラグ解除

        //初期化段階チェック
        if (InitFlag == true) return;
        InitFlag = true;

        player1 = GameManager.Instance.GetPlayer(1);
        player2 = GameManager.Instance.GetPlayer(2);

        Debug.Log("ペアチャクラムの初期化完了");
    }
    public override void Remove()
    {
        //弾の破棄
        if (FiredBullet != null)
        {
            Destroy(FiredBullet);
            FiredBullet = null;
        }
    }
}
