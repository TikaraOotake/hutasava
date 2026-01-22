using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GameManager;

public class GameManager : MonoBehaviour
{
    private GameObject Player1;
	private GameObject Player2;


	[SerializeField] int WaveLevel = 0;//ウェーブレベル
	[SerializeField] float WaveTime = 1.0f;//ウェーブ時間
	[SerializeField] float WaveTimer = 0.0f;//ウェーブタイマー

    [SerializeField] private List<WaveData_Base> WaveDataRandomList = new List<WaveData_Base>();
    [SerializeField] private int WaveData_Index;//リストの中から一つ選ぶ数字を記録する
    [SerializeField] private bool WaveClearFlag;//ウェーブをクリアしたかのフラグ


    //ウェーブ変数---
    [SerializeField] private List<GameObject> EnemyPrefabList = new List<GameObject>();//エネミーのプレハブリスト
    [SerializeField] private List<float> LevelModifierRatioList = new List<float>();//エネミーのLevel補正リスト
    [SerializeField] private List<float> SpawnRateList = new List<float>();//エネミーの出現率リスト

    [SerializeField] private List<WaveData_Base> WaveDataList = new List<WaveData_Base>();//ウェーブ
    //---------------

    [SerializeField] private EnemyManager enemyManager;//エネミーマネージャー
    [SerializeField] private UI_Manager ui_Manager;//UIマネージャー
    [SerializeField] private ItemManager itemManager;//アイテムマネージャー

    [SerializeField] private int Money;//銭
    [SerializeField] private GameObject CoinPrefab;//コインのプレハブ

    [SerializeField] private List<EquipmentItem_Base> OriginItemDataList;//複製したいアイテムを登録して使う

    [SerializeField] private RewardScene rewardScene;//報酬シーンのクラス
    [SerializeField] private UI_RewardScene ui_RewardScene;//報酬SceneのUI

    [SerializeField] private SceneAsset ClearScene;
    [SerializeField] private SceneAsset GameoverScene;
    public  enum GameSceneStatus
    {
        PlayGame,
        Pause,
        Rest,
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

        //各マネージャー取得
        enemyManager = this.GetComponent<EnemyManager>();//エネミーマネージャー取得
        ui_Manager = this.GetComponent<UI_Manager>();//UIマネージャー取得
        itemManager = this.GetComponent<ItemManager>();//アイテムマネージャー取得

        rewardScene = this.GetComponent<RewardScene>();//報酬シーンクラス取得

        // シーンをまたいでも破棄されない
        //DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        WaveTimer = WaveTime;//タイマー設定

        //休憩シーン呼び出し
        Event_Rest();

        //時間をとめる
        Time.timeScale = 0.0f;

        //報酬UIを呼び出す
        if (ui_RewardScene != null)
        {
            ui_RewardScene.gameObject.SetActive(true);
            ui_RewardScene.GenerateItem();
        }
    }
    private void Update()
    {
        switch (gameSceneStatus)
        {
            case GameSceneStatus.PlayGame:
                WaveTimer = Mathf.Max(0.0f, WaveTimer - Time.deltaTime);
                if (WaveTimer <= 0.0f)
                {
                    Event_Rest();
                }
                if (ui_Manager != null)
                {
                    ui_Manager.SetWaveProgressGaugeValue(WaveTimer, WaveTime);
                }
                break;
            case GameSceneStatus.Pause:
                break;
            case GameSceneStatus.Rest:
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button0))
                {
                    Event_PlayGame();
                }
                break;
            case GameSceneStatus.GameOver:
                break;
        }

        if (ui_Manager != null)
        {
            ui_Manager.SetMoneyValue_UI(Money);
        }
    }
    public void Event_PlayGame()
    {
        //インゲーム状態
        gameSceneStatus = GameSceneStatus.PlayGame;

        InitWave();

        //時間をもどす
        Time.timeScale = 1.0f;

        //報酬シーンのUIを閉じる
        if (ui_RewardScene != null)
        {
            ui_RewardScene.gameObject.SetActive(false);
        }
    }
    public void Event_GameOver()
    {
        //ゲームオーバーに切り替え
        gameSceneStatus = GameSceneStatus.GameOver;

        //時間をもどす
        Time.timeScale = 0.0f;

    }

    public void Event_Rest()
    {
        //報酬シーンを呼び出す
        if (rewardScene != null && WaveClearFlag == false)
        {
            rewardScene.OpenRewardScene();
        }
        WaveClearFlag = true;//クリア状態に

        //時間をとめる
        Time.timeScale = 0.0f;

        //ダメージ表示を削除
        if (ui_Manager != null) ui_Manager.CleanDamageDisplayUI();

        //ゲームシーンステータスを休憩に変更
        gameSceneStatus = GameSceneStatus.Rest;

        //全てのエネミーを削除
        if (enemyManager != null) enemyManager.DestroyAllEnemy();

        if (WaveLevel + 1 >= WaveDataList.Count)//次のウェーブがリストの端かチェック
        {
            //次のウェーブが設定されていないためステージクリアとみなす
            Debug.Log("ステージクリア");
            LoadScene(ClearScene);
            return;
        }
    }
    public void SetGameSceneStatus(GameSceneStatus _status)
    {
        gameSceneStatus = _status;
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
        Debug.Log("Waveの初期化");

        //ウェーブレベルを上げる
        ++WaveLevel;
        //ウェーブレベルをUIに反映
        if (ui_Manager != null) ui_Manager.SetWaveLevelText_UI(WaveLevel);

        //ウェーブのリストを選ぶ
        WaveData_Index = Random.Range(0, WaveDataRandomList.Count);

        //仮入れ先のウェーブデータ変数
        WaveData_Base tempWaveData = null;

        //ウェーブデータを取得
        if (WaveData_Index >= 0 && WaveData_Index < WaveDataRandomList.Count)//リスト内かチェック
        {
            tempWaveData = WaveDataRandomList[WaveData_Index];
        }
        if (WaveLevel >= 0 && WaveLevel < WaveDataList.Count)//リスト内かチェック
        {
            tempWaveData = WaveDataList[WaveLevel];
        }

        //ウェーブデータを登録
        if (enemyManager != null)
        {
            enemyManager.SetWaveData(tempWaveData);//ウェーブデータを登録
            enemyManager.SetEnemyLevel(WaveLevel);//Levelを設定
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
    public EquipmentItem_Base GetRandCopyItemData()
    {
        //リストサイズを取得し確認
        int Size = OriginItemDataList.Count;
        if (Size > 0)
        {
            //ランダムな添え字を取得
            int index = Random.Range(0, Size);
            EquipmentItem_Base item = OriginItemDataList[index];

            if (item != null)
            {
                //アイテムデータを複製して渡す
                return Instantiate(item);
            }
            else
            {
                //アイテムデータが無効なので終了
                return null;
            }
        }
        else
        {
            //リストのサイズが満たしていないため終了
            return null;
        }
    }
    public void SetHoldingUI(UI_Base _ui)
    {
        if(ui_Manager!=null)
        {
            ui_Manager.SetHoldingUI(_ui);
        }
    }

    public void GenerateDamageDisplayUI(int _Damage, Vector3 _Pos)
    {
        if (ui_Manager != null)
        {
            ui_Manager.GenerateDamageDisplayUI(_Damage, _Pos);
        }
    }
    public PlayerManager GetPlayerManager()
    {
        return GetComponent<PlayerManager>();
    }
    public void TradeItem(ItemContainer _Container, int _Index)
    {
        if (itemManager != null)
        {
            itemManager.TradeItem(_Container, _Index);
        }
    }
    public void SaleItem()
    {
        if (itemManager != null)
        {
            itemManager.SaleItem();
        }
    }

    //選択肢を選択可能状態にする
    public void SetSelectSlot_isSelective(UI_Base _ui)
    {
        if (ui_Manager != null)
        {
            ui_Manager.SetSelectSlot_isSelective(_ui);
        }
    }
    public void SetSelectSlot_isSelective(List<UI_Base> _List)
    {
        if (ui_Manager != null)
        {
            ui_Manager.SetSelectSlot_isSelective(_List);
        }
    }
    public void SetSelectSlot_isSelective(List<UI_ItemSlot_V2> _List)
    {
        if (ui_Manager != null)
        {
            List<UI_Base> tempList = new List<UI_Base>();
            for (int i = 0; i < _List.Count; ++i)
            {
                tempList.Add(_List[i]);
            }
            ui_Manager.SetSelectSlot_isSelective(tempList);
        }
        else
        {
            Debug.Log("UIマネージャーが見つかりません");
        }
    }
    //引数と同一のものを除外する
    public void RemoveSelectSlot(List<UI_Base> _List)
    {
        if (ui_Manager != null)
        {
            ui_Manager.RemoveSelectSlot(_List);
        }
    }
    public void RemoveSelectSlot(List<UI_ItemSlot_V2> _List)
    {
        if (ui_Manager != null)
        {
            List<UI_Base> tempList = new List<UI_Base>();
            for (int i = 0; i < _List.Count; ++i)
            {
                tempList.Add(_List[i]);
            }
            ui_Manager.RemoveSelectSlot(tempList);
        }
    }

    //選択肢リストを全て除外
    public void RemoveSelectSlot_all()
    {
        if (ui_Manager != null)
        {
            ui_Manager.RemoveSelectSlot_all();
        }
    }

    public UI_Manager GetUI_Manager()
    {
        return ui_Manager;
    }

    private void LoadScene(SceneAsset _scene)
    {
        string sceneName = "";

        if (_scene != null)
        {
            //取得
            sceneName = _scene.name;

            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogError("sceneNameが空！");
                return;
            }

            if (!Application.CanStreamedLevelBeLoaded(sceneName))
            {
                Debug.LogError(
                    $"Scene '{sceneName}' はBuild Profiles（SceneList）に登録されてないよ！"
                );
                return;
            }

            //読み込みを開始
            SceneManager.LoadScene(sceneName);
        }
    }
}
