using UnityEngine;

public class BondEffect : EquipmentItem_Base
{
    [SerializeField] protected GameObject Player1;
    [SerializeField] protected GameObject Player2;

    //初期化したか確認するフラグ
    protected bool InitedFalg;
    public override void Init()
    {
        //初期化済みなら終了
        if (InitedFalg == true) return;

        InitedFalg = true;//初期化済みとしてフラグをture



    }
    public virtual void Update_BondEffect()
    {
        Init();

        return;
    }
}
