using UnityEngine;

public class Coin : Item_Base
{
    [SerializeField] private int Value = 1;

    [SerializeField] private float MoveSpeed = 0.0f;//移動速度
    [SerializeField] private float AccSpeed = 1.0f;//加速度

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //取得状態
        if (Getter != null)
        {
            MoveSpeed += AccSpeed * Time.deltaTime;

            //距離を計算
            float Length = 0.0f;
            Length = Vector3.Distance(Getter.transform.position, transform.position);

            //移動方向を計算
            Vector3 moveVec = Getter.transform.position - transform.position;

            moveVec.Normalize();//正規化

            //移動を反映
            transform.position = transform.position + moveVec * MoveSpeed * Time.deltaTime;

            //移動速度が加速度を超えるか距離が移動量を超えたか
            if (MoveSpeed >= AccSpeed || Length <= MoveSpeed * Time.deltaTime)
            {
                //残高を加算
                GameManager.Instance.AddMoney(Value);

                //自身を削除
                Destroy(this.gameObject);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (Getter != null) return;

        if (other.GetComponent<PlayerController_3d>() != null)
        {
            //プレイヤーなら回収状態に
            Getter = other.gameObject;
            Debug.Log("プレイヤーと接触");
        }
    }
}
