using Unity.VisualScripting;
using UnityEngine;

public class Boar : Enemy_3d
{
    [SerializeField] private float RushTime = 1.0f;//突進時間
    [SerializeField] private float RushTimer;//突進タイマー

    [SerializeField] private Vector3 MoveVec;

    private void Start()
    {
        //移動先を探す
        SearchChasePlayer();

        Vector3 normaliVec = TargetPoint - transform.position;
        normaliVec.y = 0.0f;//縦軸を無視
        MoveVec = normaliVec.normalized;//正規化
    }
    void Update()
    {
        if (RushTimer <= 0.0f)
        {
            //移動先を探す
            SearchChasePlayer();

            Vector3 normaliVec = TargetPoint - transform.position;
            normaliVec.y = 0.0f;//縦軸を無視
            MoveVec = normaliVec.normalized;//正規化

            //タイマーセット
            RushTimer = RushTime;
        }



        //移動
        //Move();
        transform.position += MoveVec * MoveSpeedPoint_Result * Time.deltaTime;

        Update_Blow();

        //タイマー更新
        RushTimer = Mathf.Max(0.0f, RushTimer - Time.deltaTime);
    }
}
