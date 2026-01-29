//using NUnit.Framework;
using StructStatus;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_3d : Character
{
    [SerializeField]
    private int PlayerNumber = 0;

    [SerializeField]
    private GameObject Camera;

    float StickInputCancelTimer;//スティック入力をキャンセルし続けるタイマー

    [SerializeField] UI_GaugeBar HP_Gauge_Comp;//HPゲージのコンポ(仮)

    [SerializeField] List<Weapon> WeaponList = new List<Weapon>();
    [SerializeField] List<Accessory> AccessoryList = new List<Accessory>();

    [SerializeField] Weapon TestOriginWeapon;//テスト用の複製元の武器

    [SerializeField] float RevivalTime_Base = 1.0f;//基礎復活時間
    [SerializeField] float RevivalTime_Result = 1.0f;//結果復活時間
    [SerializeField] float RevivalTimer;//復活タイマー
    [SerializeField] float RevivalTimer_old;//前フレームの復活タイマーを記録

    [SerializeField] private float HealthPoint_Current_old;//前フレームのHP状態を記録

    //アイテムを格納するコンテナ
    [SerializeField] private ItemContainer itemContainer;

    [SerializeField] private PlayerManager playerManager;//プレイヤーマネージャー
    [SerializeField] private QuadTextureAnimation quadTextureAnimation;//テクスチャアニメーション

    [SerializeField] private List<Texture> PlayerTextureFrontList = new List<Texture>();//前向きのテクスチャリスト
    [SerializeField] private List<Texture> PlayerTextureBackList = new List<Texture>();//後ろ向きのテクスチャリスト
    bool IsMove;//アニメーション用歩行状態確認フラグ


    [SerializeField] Rigidbody rigidbody;

    //削除予定---------------------------
    [SerializeField] private UI_Inventory_Player uI_Inventory_Player;

    private void Awake()
    {
        
    }
    void Start()
    {
        Camera = GameObject.FindGameObjectWithTag("MainCamera");

        //自身をゲームマネージャーに登録
        GameManager.Instance.SetPlayer(PlayerNumber, this.gameObject);

        //プレイヤーマネージャー取得
        if (playerManager == null) playerManager = GameManager.Instance.GetPlayerManager();


        if (TestOriginWeapon != null)
        {
            WeaponList.Add(Instantiate(TestOriginWeapon));//複製したものを登録
        }

        //リジットボディ取得
        rigidbody = GetComponent<Rigidbody>();

        //アイテムのUIを選択可能に
        if (itemContainer != null)
        {
            List<UI_ItemSlot_V2> uiList = itemContainer.GetItem_DisplayUI_List();
            //Debug.Log(uiList.Count + "個のUIを選択可能状態にします");

            //クリック時のEventを設定
            itemContainer.SetClickEvent(itemContainer.TradeItem);

            //選択可能状態にしたい選択肢を登録
            GameManager.Instance.SetSelectSlot_isSelective(uiList);
        }
        else
        {
            Debug.Log("アイテムストレージがありません");
        }

        //ステータス計算
        CalcuStatus();
    }

    // Update is called once per frame
    void Update()
    {
        if (rigidbody != null)
        {
            rigidbody.linearVelocity = Vector3.zero;
        }

        if (IsDead == false)
        {
            Move();
            Update_Weapons();


            //HPをUIに反映
            if (HP_Gauge_Comp != null) HP_Gauge_Comp.SetGaugeValue(HealthPoint_Current, HealthPointMax_Result);
        }
        else
        {
            //復活進捗をUIに反映
            if (HP_Gauge_Comp != null) HP_Gauge_Comp.SetGaugeValue(RevivalTime_Result - RevivalTimer, RevivalTime_Result);
        }


        Update_Animation();//Animation更新
        Update_Blow();
        

        //タイマー更新
        RevivalTimer = Mathf.Max(0.0f, RevivalTimer - Time.deltaTime);

        if (RevivalTimer <= 0.0f && RevivalTimer != RevivalTimer_old)
        {
            //HPを全回復
            HealthPoint_Current = HealthPointMax_Result;

            //死亡フラグ
            IsDead = false;
        }

        //HPが0になった瞬間
        if (HealthPoint_Current <= 0.0f && HealthPoint_Current != HealthPoint_Current_old)
        {
            IsDead = true;
        }

        //変数の状態を記録
        HealthPoint_Current_old = HealthPoint_Current;
        RevivalTimer_old = RevivalTimer;
    }

    private void Update_Weapons()
    {
        if (Time.timeScale == 0.0f) return;

        for (int i = 0; i < WeaponList.Count; i++)
        {
            if (WeaponList[i] != null)
            {
                WeaponList[i].Update_Item(this.gameObject);
            }
        }

        if (uI_Inventory_Player != null)
        {
            List<GameObject> weapons = uI_Inventory_Player.GetPlayerItemSlot();
            for (int i = 0; i < weapons.Count; i++)
            {
                if (weapons[i] != null)
                {
                    UI_SelectSlot slot = weapons[i].GetComponent<UI_SelectSlot>();
                    if (slot != null)
                    {
                        EquipmentItem_Base item = slot.GetItem();
                        if (item != null)
                        {
                            Weapon weapon = (Weapon)item;

                            if (weapon != null)
                            {
                                weapon.Update_Item(this.gameObject);
                            }
                        }
                    }
                }
            }
        }

        if (itemContainer != null)
        {
            List<EquipmentItem_Base> itemList = itemContainer.GetItemList();
            for (int i = 0; i < itemList.Count; ++i)
            {
                if (itemList[i] is Weapon)
                {
                    Weapon weapon = (Weapon)itemList[i];
                    weapon.Update_Item(this.gameObject);
                }
            }
        }
    }

    private void Update_Animation()
    {
        //有効性確認
        if (quadTextureAnimation == null) return;

        float Angle = transform.rotation.eulerAngles.y;//角度取得

        //カメラの角度を減算
        if (Camera != null) Angle -= Camera.transform.eulerAngles.y;

        Angle += 90;//90度ずらす

        //360度の範囲に補正
        Angle = ((Angle + 360.0f) % 360.0f);

        //キャラクターの左右反転
        if (Angle > 10 && Angle < 170)
        {
            quadTextureAnimation.SetFlipX(true);
        }
        else if (Angle > 190 && Angle < 350)
        {
            quadTextureAnimation.SetFlipX(false);
        }

        //キャラクターの前後切り替え
        if (Angle >= 0 && Angle < 80 || Angle > 280 && Angle <= 360)
        {
            quadTextureAnimation.SetTexture(PlayerTextureBackList);
        }
        else if (Angle > 100 && Angle < 260)
        {
            quadTextureAnimation.SetTexture(PlayerTextureFrontList);
        }
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
                MoveVec.x -= 1.0f;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                MoveVec.x += 1.0f;
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

        //歩行状態か記録
        IsMove = MoveVec != Vector2.zero;

        //正規化
        MoveVec.Normalize();

        //タイマー更新
        StickInputCancelTimer = Mathf.Max(StickInputCancelTimer - Time.deltaTime, 0.0f);

        //Cameraの角度に合わせて移動方向を回転させる
        float angle = 0.0f;                                                  //変数宣言
        if (Camera != null) angle = -Camera.transform.rotation.eulerAngles.y;//取得
        MoveVec = Quaternion.Euler(0, 0, angle) * MoveVec;                   //補正

        //座標取得
        Vector2 Pos = new Vector2(transform.position.x, transform.position.z);

        //移動量計算
        Pos += MoveVec * MoveSpeedPoint_Result * Time.deltaTime;

        //移動を開始
        transform.position = new Vector3(Pos.x, transform.position.y, Pos.y);

        //プレイヤーを移動する方向を向く
        if (MoveVec != Vector2.zero)//移動入力がある場合のみ
        {
            Vector3 PlayerAngle = transform.eulerAngles;
            PlayerAngle.y = -Mathf.Atan2(MoveVec.y, MoveVec.x) * Mathf.Rad2Deg;//ベクトルを角度(度数に変換)
            transform.eulerAngles = PlayerAngle;
        }

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
    public void SetAccessory(Accessory _Accessory)
    {
        AccessoryList.Add(_Accessory);//追加

        //ステータス再計算
        CalcuStatus();
    }

    public override void SetDamage(float _Damage)
    {
        //死亡状態ならダメージを通さない
        if (HealthPoint_Current <= 0.0f) return;

        //現在HPが最大HPを超えないように補正
        HealthPoint_Current = Mathf.Min(HealthPointMax_Result, HealthPoint_Current);

        //ダメージ
        HealthPoint_Current = Mathf.Max(0.0f, HealthPoint_Current - _Damage);

        //死亡状態なら復活タイマーセット
        if (HealthPoint_Current <= 0.0f)
        {
            RevivalTimer = RevivalTime_Result;
        }
    }
    public override void CalcuStatus()
    {
        float AcceAtk = 0.0f;
        float AcceHP = 0.0f;
        float AcceSpeed = 0.0f;

        //アクセサリステータスを全て加算
        if (uI_Inventory_Player != null)//没
        {
            List<Accessory> accessoryList = uI_Inventory_Player.GetAccessoryList();
            for (int i = 0; i < accessoryList.Count; ++i)
            {
                if (accessoryList[i] != null)
                {
                    CharacterStatus status = accessoryList[i].GetStatus();
                    AcceAtk += status.AttackPoint;
                    AcceHP += status.HealthPoint;
                    AcceSpeed += status.SpeedPoint;
                }
            }
        }
        for (int i = 0; i < AccessoryList.Count; ++i)
        {
            if (AccessoryList[i] != null)
            {
                CharacterStatus status = AccessoryList[i].GetStatus();
                AcceAtk += status.AttackPoint;
                AcceHP += status.HealthPoint;
                AcceSpeed += status.SpeedPoint;
            }
        }

        MoveSpeedPoint_Result = MoveSpeedPoint_Base + MoveSpeedPoint_Base * AcceSpeed;
        AttackPoint_Result = AttackPoint_Base + AttackPoint_Base * AcceAtk;
        //DefensePoint_Result = DefensePoint_Base;
        HealthPointMax_Result = HealthPointMax_Base + HealthPointMax_Base * AcceHP;

        //ステータス表示を更新
        playerManager.Update_PlayerStatusView();
    }

    protected override void IsHit(GameObject _HitObj)
    {
        //死亡状態なら終了
        if (IsDead) return;

        //仮ダメージ処理
        if (_HitObj.tag == "Enemy")//衝突相手がエネミー
        {
            Character chara = _HitObj.GetComponent<Character>();
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
                BlowVec *= BlowPower;

                //ノックバックを反映
                chara.SetKnockBack(new Vector2(BlowVec.x, BlowVec.z));
            }
        }
    }

    public ItemContainer GetItemContainer()
    {
        return itemContainer;
    }
}
