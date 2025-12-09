using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameObject Player1;
	private GameObject Player2;


	[SerializeField] int WaveLevel = 0;//ウェーブレベル
	[SerializeField] float WaveTime = 1.0f;//ウェーブ時間
	[SerializeField] float WaveTimer = 0.0f;//ウェーブタイマー

    [SerializeField] private List<WaveData_Base> WaveDataList = new List<WaveData_Base>();
    [SerializeField] private int WaveData_Index;//リストの中から一つ選ぶ数字を記録する
    [SerializeField] private bool WaveClearFlag;//ウェーブをクリアしたかのフラグ


    //ウェーブ変数---
    [SerializeField] private List<GameObject> EnemyPrefabList = new List<GameObject>();//エネミーのプレハブリスト
    [SerializeField] private List<float> LevelModifierRatioList = new List<float>();//エネミーのLevel補正リスト
    [SerializeField] private List<float> SpawnRateList = new List<float>();//エネミーの出現率リスト
    //---------------

    [SerializeField] private EnemyManager enemyManager;//エネミーマネージャー
    [SerializeField] private UI_Manager ui_Manager;//UIマネージャー

    [SerializeField] private int Money;//銭
    [SerializeField] private GameObject CoinPrefab;//コインのプレハブ

    enum GameSceneStatus
    {
        PlayGame,
        Pause,
        GameOver
    }
    [SerializeField] GameSceneStatus gameSceneStatus;


    public static GameManager Instance { get; private set; }

    [SerializeField] private Weapon PairWeapon;//ペア武器

    private void Awake()
    {
        // すでにインスタンスがある → 自分を破棄して終了
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // インスタンスが存在しない → 自分を設定
        Instance = this;

        // シーンをまたいでも破棄されない
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        WaveTimer = WaveTime;//タイマー設定

        enemyManager = this.GetComponent<EnemyManager>();//エネミーマネージャー取得
        ui_Manager = this.GetComponent<UI_Manager>();//UIマネージャー取得
    }
    private void Update()
    {
        WaveTimer = Mathf.Max(0.0f, WaveTimer - Time.deltaTime);

        if (WaveTimer <= 0.0f)
        {
            WaveClearFlag = true;//クリア状態に

            //全てのエネミーを削除
            //if (enemyManager != null) enemyManager.DestroyAllEnemy();
        }


        //Wave初期化
        if (WaveClearFlag == true)
        {
            //報酬UIを呼び出す

            //
            InitWave();
        }

        //Wave進行
        if (WaveClearFlag == false)
        {
            //=WaveDataList[WaveData_Index].GetSpawnRateList();


        }

        if(ui_Manager!=null)
        {
            ui_Manager.SetMoneyValue_UI(Money);
        }
	}

    public void SetMoney(int _Money)
    {
        Money = _Money;
    }
    public int GetMoney()
    {
        return Money;
    }
    public void AddMoney(int _Money)
    {
        Money += _Money;
    }
    public void SetCursorObj(GameObject _Cursor)
    {
        if (ui_Manager) ui_Manager.SetCursorObj(_Cursor);
    }
    public GameObject GetCursorObj()
    {
        if (ui_Manager) return ui_Manager.GetCursorObj();
        else
            return null;
    }
    private void InitWave()
    {
        //ウェーブレベルを上げる
        ++WaveLevel;

        //ウェーブのリストを選ぶ
        WaveData_Index = Random.Range(0, WaveDataList.Count);

        //ウェーブDataをセット
        if (WaveData_Index >= 0 && WaveData_Index < WaveDataList.Count)//リスト内かチェック
        {
            if (enemyManager != null)
            {
                enemyManager.SetWaveData(WaveDataList[WaveData_Index]);//リストを登録
                enemyManager.SetEnemyLevel(WaveLevel);//Levelを設定
            }
        }
        else
        {
            if (enemyManager != null) enemyManager.SetWaveData(null);//登録
        }


        //フラグを戻す
        WaveClearFlag = false;

        WaveTimer = WaveTime;//タイマー設定
    }

    public int GetWaveLevel()
    {
        return WaveLevel;
    }

    //Player番号から任意のプレイヤーを取得
    public GameObject GetPlayer(int _PlayerNumber)
    {
        GameObject player = null;
        if (_PlayerNumber == 1)
        {
            player = Player1;
        }
        else if (_PlayerNumber == 2)
        {
            player = Player2;
        }
        else
        {
            Debug.Log("無効なプレイヤー番号を検出しました　nullを返します");
        }

            return player;
    }
    //Player番号から任意のプレイヤーを設定
    public void SetPlayer(int _PlayerNumber, GameObject _Player)
    {
        if (_PlayerNumber == 1)
        {
            Player1 = _Player;
        }
        else if (_PlayerNumber == 2)
        {
            Player2 = _Player;
        }
        else
        {
            Debug.Log("無効なプレイヤー番号を検出しました");
        }
    }

    //プレイヤー同士の中点を取得
    public Vector3 GetPlayerMiddlePos()
    {
        Vector3 Player1Pos = Vector3.zero;
        Vector3 Player2Pos = Vector3.zero;

        //座標の取得
        if (Player1) Player1Pos = Player1.transform.position;
        if (Player2) Player2Pos = Player2.transform.position;

        if (Player1 == null && Player2 == null) return Vector3.zero;//どちらも無効であれば0を返す
        else if (Player1 != null && Player2 == null) return Player1Pos;
        else if (Player1 == null && Player2 != null) return Player2Pos;

        //座標を調べる
        Vector3 midPos = Player1Pos + (Player2Pos - Player1Pos) * 0.5f;//二点間の中心を調べる

        return midPos;
    }

    //計算関数　※移動予定
    public static Vector2 AngleToVector(float angleDeg)
    {
        float rad = angleDeg * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
    }
    public static float VectorToAngle(Vector2 v)
    {
        return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
    }

    public void DeleteEnemy(GameObject _Enemy)
    {
        if (enemyManager == null) return;//ヌルチェック
        enemyManager.Exclusion_EnemyList(_Enemy);
    }
    public GameObject GetNearestEnemy(Vector3 _Pos)
    {
        if (enemyManager == null) return null;//ヌルチェック
        return enemyManager.GetNearestEnemy(_Pos);
    }

    public void DropItem_Coin(Vector3 _pos)
    {
        if (CoinPrefab != null)
        {
            GameObject obj = Instantiate(CoinPrefab);
            obj.transform.position = _pos;
        }
    }
}
