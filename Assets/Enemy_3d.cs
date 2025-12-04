using System.Collections.Generic;
using UnityEngine;


public class Enemy_3d : Character
{
    [SerializeField] private Vector3 TargetPoint;//移動目標点

    private GameObject Player1;
    private GameObject Player2;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //移動先を探す
        SearchChasePlayer();

        //移動
        Move();

        Update_Blow();
    }

    public override void SetDamage(float _Damage)
    {
        HealthPoint_Current = Mathf.Max(0.0f, HealthPoint_Current - _Damage);

        //HPが0であれば自身を削除
        if (HealthPoint_Current <= 0.0f)
        {
            Destroy(this.gameObject);
        }
    }

    //追跡するPlayerを探す
    void SearchChasePlayer()
    {
        var p1 = GameManager.Instance.GetPlayer(1);
        var p2 = GameManager.Instance.GetPlayer(2);

        Transform best = null;
        float bestDist = float.MaxValue;

        void CheckPlayer(GameObject p)
        {
            if (p == null) return;
            var c = p.GetComponent<Character>();
            if (c == null || c.GetHealthPoint_Current() <= 0) return;

            float d = Vector3.Distance(p.transform.position, transform.position);
            if (d < bestDist)
            {
                bestDist = d;
                best = p.transform;
            }
        }

        CheckPlayer(p1);
        CheckPlayer(p2);

        if (best != null)
            TargetPoint = best.position;
    }
    private void Move()
    {
        Vector3 normaliVec = TargetPoint - transform.position;
        normaliVec.y = 0.0f;//縦軸を無視
        normaliVec.Normalize();//正規化
        transform.position += normaliVec * MoveSpeedPoint_Result * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        //ダメージ処理
        if (other.tag == "Player")//衝突相手がプレイヤー
        {
            Character chara = other.GetComponent<Character>();
            if (chara != null)
            {
                //ダメージ計算
                float Def = chara.GetDefensePoint_Result();

                float Damage = AttackPoint_Result;//補正は要件等

                //ダメージ適用
                chara.SetDamage(Damage);

                //吹き飛ぶ方向を計算
                Vector3 BlowVec = chara.transform.position - transform.position;//方向を算出
                BlowVec.Normalize();//正規化
                BlowVec *= 0.5f;//いい感じに補正

                //ノックバックを反映
                chara.SetKnockBack(new Vector2(BlowVec.x, BlowVec.z));
            }
        }
    }
}
