
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "WaveData_Scenario", menuName = "ScriptableObjects/WaveData_Scenario")]
public class WaveData_Scenario : WaveData_Base
{
    [SerializeField] private int ScenarioPhaseIndex;//シナリオ進行段階番号
    [SerializeField] private List<float> ScenarioPhaseTimeList;//シナリオ進行時間リスト(経過後次のフェーズに移る)
    [SerializeField] private List<GameObject> PhaseSpawnEnemyList;//各フェーズ開始時に出現するエネミーのプレハブ
    [SerializeField] private List<int> PhaseSpawnNamList;//各フェーズ開始時に出現するエネミーの数
    [SerializeField] private List<int> PhaseSpawnPosIndexList;//各フェーズどこの座標にエネミーが出現するかみるIndexのリスト(配列外の場合はランダム)

    private bool ClearFlag;

    [SerializeField]
    private float PhaseTimer;

    //リスト同期用変数
    private int prev1, prev2, prev3, prev4;

    public override void Init()
    {
        Debug.Log("シナリオデータを初期化します");

        //フェーズ初期化
        ScenarioPhaseIndex = 0;

        //クリアフラグをリセット
        ClearFlag = false;

        //サイズチェック
        if (0 <= ScenarioPhaseTimeList.Count && ScenarioPhaseTimeList.Count > ScenarioPhaseIndex)
        {
            //タイマー設定
            PhaseTimer = ScenarioPhaseTimeList[ScenarioPhaseIndex];
        }

        //敵を生成
        SpawnEnemy(ScenarioPhaseIndex);
    }
    public override void Update_Wave()
    {
        if (ClearFlag == true) return;

        //タイマー経過
        PhaseTimer = Mathf.Max(0.0f, PhaseTimer - Time.deltaTime);

        //タイマー終了を確認
        if (PhaseTimer <= 0.0f)
        {
            Debug.Log("フェーズを移行");

            //フェーズの初期化処理

            //フェーズインクリメント
            ++ScenarioPhaseIndex;

            //敵を生成
            SpawnEnemy(ScenarioPhaseIndex);

            //サイズチェック
            if (0 <= ScenarioPhaseIndex && ScenarioPhaseTimeList.Count > ScenarioPhaseIndex)
            {
                //タイマー設定
                PhaseTimer = ScenarioPhaseTimeList[ScenarioPhaseIndex];
            }
            else
            {
                //配列外なのでクリアとして判定
                Debug.Log("ウェーブクリア");

                //クリア状態に
                ClearFlag = true;

                //休憩シーン呼び出し
                GameManager.Instance.Event_Rest();
            }
        }
    }

    private void SpawnEnemy(int _Index)
    {
        GameObject enemyPrefab = null;//プレハブ
        Vector2 Pos = Vector2.zero;//座標
        int SpawnNam = 1;//出現数

        //Prefab取得
        if (0 <= _Index && PhaseSpawnEnemyList.Count > _Index)//サイズチェック
        {
            enemyPrefab = PhaseSpawnEnemyList[_Index];
        }
        else
        {
            Debug.Log("エネミー取得に失敗");
        }

        //座標取得
        if (0 <= _Index && PhaseSpawnPosIndexList.Count > _Index)//サイズチェック
        {
            //座標インデックスの取得
            int PosIndex = PhaseSpawnPosIndexList[_Index];

            if (0 <= PosIndex && SpawnPosList.Count > PosIndex)
            {
                Pos = SpawnPosList[PosIndex];//取得
            }
        }
        else
        {
            //該当する要素が無い為ランダムな座標に出現

            float SpawnLength = 20.0f;//出現距離

            //ランダムな方向にずらすベクトルを生成
            Vector2 randVec = Vector3.zero;
            randVec.x = Random.Range(-1.0f, 1.0f);
            randVec.y = Random.Range(-1.0f, 1.0f);

            randVec.Normalize();//正規化
            randVec *= SpawnLength;//倍化

            //代入
            Pos = randVec;
        }

        //出現数取得
        if (0 <= _Index && PhaseSpawnNamList.Count > _Index)//サイズチェック
        {
            SpawnNam = PhaseSpawnNamList[_Index];
        }


        //エネミーマネージャーの取得
        EnemyManager enemyManager = GameManager.Instance.GetEnemyManager();

        //エネミー生成
        if (enemyPrefab != null)//有効性確認
        {
            //出現数分繰り返し
            for (int i = 0; i < SpawnNam; ++i)
            {
                GameObject enemy = Instantiate(enemyPrefab);//生成

                //座標代入
                Vector3 enemyPos = enemy.transform.position;
                enemyPos.x = Pos.x + i;//出現数に応じてずらす
                enemyPos.z = Pos.y;
                enemy.transform.position = enemyPos;

                //マネージャーに登録
                if (enemyManager != null)
                {
                    enemyManager.AddEnemyList(enemy);
                }
            }
        }
    }

    private new void OnValidate()
    {
        //リストサイズ同期用処理
        base.OnValidate();

        // どれが変更されたか判定
        bool aChanged = ScenarioPhaseTimeList.Count != prev1;
        bool bChanged = PhaseSpawnEnemyList.Count != prev2;
        bool cChanged = PhaseSpawnNamList.Count != prev3;
        bool dChanged = PhaseSpawnPosIndexList.Count != prev4;

        // 変更されたリストのサイズを基準にする
        int targetSize = -1;

        if (aChanged) targetSize = ScenarioPhaseTimeList.Count;
        else if (bChanged) targetSize = PhaseSpawnEnemyList.Count;
        else if (cChanged) targetSize = PhaseSpawnNamList.Count;
        else if (dChanged) targetSize = PhaseSpawnPosIndexList.Count;
        else return; // 何も変わってなければ終了

        // サイズ同期
        SyncSize(ScenarioPhaseTimeList, targetSize);
        SyncSize(PhaseSpawnEnemyList, targetSize);
        SyncSize(PhaseSpawnNamList, targetSize, 1);
        SyncSize(PhaseSpawnPosIndexList, targetSize, -1);

        // サイズを更新して次回比較に使う
        prev1 = ScenarioPhaseTimeList.Count;
        prev2 = PhaseSpawnEnemyList.Count;
        prev2 = PhaseSpawnNamList.Count;
        prev4 = PhaseSpawnPosIndexList.Count;
    }
}
