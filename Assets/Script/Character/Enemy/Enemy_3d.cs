using System.Collections.Generic;
using UnityEngine;


public class Enemy_3d : Character
{
    [SerializeField] protected Vector3 TargetPoint;//移動目標点

    protected GameObject Player1;
    protected GameObject Player2;
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

        //ダメージ表示を依頼
        GameManager.Instance.GenerateDamageDisplayUI((int)_Damage, transform.position);

        //HPが0であれば自身を削除
        if (HealthPoint_Current <= 0.0f)
        {
            //コインを落とす
            GameManager.Instance.DropItem_Coin(transform.position);

            //削除依頼
            GameManager.Instance.DeleteEnemy(this.gameObject);
        }
    }

    //追跡するPlayerを探す
    protected void SearchChasePlayer()
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
    protected void Move()
    {
        Vector3 normaliVec = TargetPoint - transform.position;
        normaliVec.y = 0.0f;//縦軸を無視
        normaliVec.Normalize();//正規化
        transform.position += normaliVec * MoveSpeedPoint_Result * Time.deltaTime;
    }

    public void SetLevelStatus(int _Level)
    {
        MoveSpeedPoint_Result = MoveSpeedPoint_Base;
        AttackPoint_Result = AttackPoint_Base * Mathf.Pow(1.10f, _Level);
        //DefensePoint_Result = DefensePoint_Base;
        HealthPointMax_Result = HealthPointMax_Base * Mathf.Pow(1.15f, _Level);

        //最大HPまで回復
        HealthPoint_Current = HealthPointMax_Result;
    }

    protected override void IsHit(GameObject _HitObj)
    {
        //ダメージ処理
        if (_HitObj.tag == "Player")//衝突相手がプレイヤー
        {
            Character chara = _HitObj.GetComponent<Character>();
            if (chara != null)
            {
                //死亡状態な終了
                if (chara.GetIsDead()) return;

                //ダメージ計算
                float Def = chara.GetDefensePoint_Result();

                float Damage = AttackPoint_Result;//補正は要件等

                //ダメージ適用
                chara.SetDamage(Damage);

                //吹き飛ぶ方向を計算
                Vector3 BlowVec = chara.transform.position - transform.position;//方向を算出
                BlowVec.Normalize();//正規化
                BlowVec *= BlowPower;

                //ノックバックを反映
                chara.SetKnockBack(new Vector2(BlowVec.x, BlowVec.z));
            }
        }
    }
}
