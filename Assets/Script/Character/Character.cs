using UnityEngine;

public class Character : MonoBehaviour
{
    //移動速度
    [SerializeField] protected float MoveSpeedPoint_Base = 1.0f;
    [SerializeField] protected float MoveSpeedPoint_Result = 1.0f;

    //攻撃力
    [SerializeField] protected float AttackPoint_Base = 1.0f;
    [SerializeField] protected float AttackPoint_Result = 1.0f;

    //防御力
    [SerializeField] protected float DefensePoint_Base = 1.0f;
    [SerializeField] protected float DefensePoint_Result = 1.0f;

    //最大体力
    [SerializeField] protected float HealthPointMax_Base = 1.0f;
    [SerializeField] protected float HealthPointMax_Result = 1.0f;

    //現在体力
    [SerializeField] protected float HealthPoint_Current = 1.0f;

    [SerializeField] protected float BlowPower = 10.0f;//吹き飛ばし力の程度

    [SerializeField][Range(0f, 1f)] 
    protected float BlowResist = 0.0f;// 吹き飛び抵抗 0：無抵抗 1：完全抵抗

    [SerializeField] protected Vector2 BlowVec;//吹き飛ぶ方向

    protected bool IsDead;//死亡状態フラグ

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    //吹き飛びの更新
    protected void Update_Blow()
    {
        //移動
        transform.position += new Vector3(BlowVec.x, 0.0f, BlowVec.y) * Time.deltaTime;

        const float LerpLevel = 10.0f;//滑らか度

        //滑らかに弱めるように補正
        BlowVec = Vector2.Lerp(BlowVec, Vector2.zero, 1 - Mathf.Exp(-LerpLevel * Time.deltaTime));
    }

    public virtual void SetDamage(float _Damage)
    {
        //現在HPが最大HPを超えないように補正
        HealthPoint_Current = Mathf.Min(HealthPointMax_Result, HealthPoint_Current);

        //ダメージを与える
        HealthPoint_Current = Mathf.Max(0.0f, HealthPoint_Current - _Damage);

        //ダメージ表示を依頼
        GameManager.Instance.GenerateDamageDisplayUI((int)_Damage, transform.position);
    }
    public void SetKnockBack(Vector2 _BlowVec)
    {
        BlowVec = _BlowVec * (1.0f - BlowResist);
    }

    //各種ステータス計算
    public virtual void CalcuStatus()
    {
        MoveSpeedPoint_Result = MoveSpeedPoint_Base;
        AttackPoint_Result = AttackPoint_Base;
        DefensePoint_Result = DefensePoint_Base;
        HealthPointMax_Result = HealthPointMax_Base;
    }

    public float GetAttackPoint_Result()
    {
        return AttackPoint_Result;
    }
    public float GetDefensePoint_Result()
    {
        return DefensePoint_Result;
    }
    public float GetHealthPointMax_Result()
    {
        return HealthPointMax_Result;
    }
    public  float GetMoveSpeedPoint_Result()
    {
        return MoveSpeedPoint_Result;
    }
    public float GetHealthPoint_Current()
    {
        return HealthPoint_Current;
    }
    public bool GetIsDead()
    {
        return IsDead;
    }

    protected virtual void IsHit(GameObject _HitObj)
    {
        return;
    }

    private void OnTriggerEnter(Collider other)
    {
        IsHit(other.gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        IsHit(collision.gameObject);
    }
}
