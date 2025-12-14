using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Character
{
    [SerializeField]
    private int PlayerNumber = 0;

    [SerializeField]
    private GameObject Camera;

    float StickInputCancelTimer;//スティック入力をキャンセルし続けるタイマー

    void Start()
    {
        Camera = GameObject.FindGameObjectWithTag("MainCamera");

        //自身をゲームマネージャーに登録
        GameManager.Instance.SetPlayer(PlayerNumber, this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        Update_Blow();
    }

    private void Move()
    {
        //移動方向
        Vector2 MoveVec = new Vector2(0.0f, 0.0f);

        if (PlayerNumber == 1)
        {
            if (Input.GetKey(KeyCode.W))
            {
                MoveVec.y += 1.0f;
            }
            if (Input.GetKey(KeyCode.S))
            {
                MoveVec.y -= 1.0f;
            }
            if (Input.GetKey(KeyCode.D))
            {
                MoveVec.x += 1.0f;
            }
            if (Input.GetKey(KeyCode.A))
            {
                MoveVec.x -= 1.0f;
            }
        }
        else if (PlayerNumber == 2)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                MoveVec.y += 1.0f;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                MoveVec.y -= 1.0f;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                MoveVec.x += 1.0f;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                MoveVec.x -= 1.0f;
            }
        }
        



        //キーボード入力が無い場合ジョイスティックの入力を適用
        if (MoveVec.x == 0.0f && MoveVec.y == 0.0f)
        {
            if (StickInputCancelTimer <= 0.0f)
            {
                MoveVec = GetStickInputValue();//ジョイスティックの入力を取得
            }
        }
        else
        {
            StickInputCancelTimer = 0.5f;//タイマー設定
        }

        //正規化
        MoveVec.Normalize();

        //タイマー更新
        StickInputCancelTimer = Mathf.Max(StickInputCancelTimer - Time.deltaTime, 0.0f);

        //Cameraの角度に合わせて移動方向を回転させる
        float angle = 0.0f;                                                 //変数宣言
        if (Camera != null) angle = Camera.transform.rotation.eulerAngles.z;//取得
        MoveVec = Quaternion.Euler(0, 0, angle) * MoveVec;                  //補正

        //座標取得
        Vector2 Pos = transform.position;

        //移動量計算
        Pos += MoveVec * MoveSpeedPoint_Result * Time.deltaTime;

        //移動を開始
        transform.position = Pos;//
    }

    private Vector2 GetStickInputValue()
    {
        Vector2 InputValue = new Vector2(0.0f, 0.0f);

        if (PlayerNumber == 1)
        {
            InputValue.x = Input.GetAxis("Horizontal");
            InputValue.y = Input.GetAxis("Vertical");
        }
        else if (PlayerNumber == 2)
        {
            InputValue.x = Input.GetAxis("Horizontal_R");
            InputValue.y = -Input.GetAxis("Vertical_R");//Y軸のみ反転
        }


        return InputValue;
    }
}
