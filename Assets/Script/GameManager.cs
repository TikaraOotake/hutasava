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

    public static GameManager Instance { get; private set; }

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
            //ウェーブレベルを上げる
            ++WaveLevel;

            //ウェーブのリストを選ぶ
            WaveData_Index = Random.Range(0, WaveDataList.Count);

            //ウェーブDataをセット
            if (WaveData_Index >= 0 && WaveData_Index < WaveDataList.Count)//リスト内かチェック
            {
                if (enemyManager != null) enemyManager.SetWaveData(WaveDataList[WaveData_Index]);//登録
            }
            else
            {
                if (enemyManager != null) enemyManager.SetWaveData(null);//登録
            }
          

            //フラグを戻す
            WaveClearFlag = false;

            WaveTimer = WaveTime;//タイマー設定
        }

        //Wave進行
        if (WaveClearFlag == false)
        {
            //=WaveDataList[WaveData_Index].GetSpawnRateList();


        }
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
}
