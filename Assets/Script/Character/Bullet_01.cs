using UnityEngine;

public class Bullet_01 : MonoBehaviour
{
    [SerializeField] private float BulletSpeed = 1.0f;
    [SerializeField] private float BulletAtk = 1.0f;

    [SerializeField] private Vector3 BulletMoveVec;

    [SerializeField] private float DestroyTimer = 1.0f;//削除タイマー

	[SerializeField] private bool isPerforate;//弾貫通フラグ
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0.0f, 0.0f, BulletSpeed) * Time.deltaTime);

        //タイマー更新
        DestroyTimer = Mathf.Max(0.0f, DestroyTimer - Time.deltaTime);

        if (DestroyTimer <= 0.0f)
        {
            Destroy(this.gameObject);
        }
    }

    public void SetAtkValue(float _Atk)
    {
        BulletAtk = _Atk;
    }

	private void OnTriggerEnter(Collider other)
	{
		//仮ダメージ処理
		if (other.tag == "Enemy")//衝突相手がエネミー
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

                //自身を削除
                if (isPerforate == false) Destroy(this.gameObject);
            }
        }
    }
}
