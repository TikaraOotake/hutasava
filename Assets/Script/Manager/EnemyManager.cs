using NUnit.Framework;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
//using static UnityEditor.Progress;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] 
    private float SpawnEnemyRateTime = 1.0f;//敵生成間隔
    private float SpawnEnemyRateTimer = 0.0f;//敵生成タイマー

    [SerializeField]
    private List<GameObject> EnemyList = new List<GameObject>();//生成したエネミーのリスト

    [SerializeField] private GameObject TestEnemyPrefab;//テスト用エネミーのプレハブ

    [SerializeField] private float SpawnLength = 1.0f;//エネミー出現距離

    [SerializeField] private WaveData_Base WaveData;

    [SerializeField] private int EnemyLevel;

    [SerializeField] private float SpawnEnemyHeight;//出現する高さ

    void Start()
    {
        SpawnEnemyRateTimer = SpawnEnemyRateTime;
    }

    void Update()
    {
        if (WaveData != null)
        {
            //シナリオデータだったら専用の処理に切り換える
            if(WaveData is WaveData_Scenario)
            {
                WaveData.Update_Wave();
                return;//終了
            }

            if (SpawnEnemyRateTimer <= 0.0f)
            {
                //エネミーを取得
                GameObject SpawnEnemy = WaveData.GetSpawnEnemyRand();

                //エネミー生成
                if (SpawnEnemy != null)
                {
                    //中心座標を取得
                    Vector3 midPos = GameManager.Instance.GetPlayerMiddlePos();

                    //ランダムな方向にずらすベクトルを生成
                    Vector3 randVec = Vector3.zero;
                    randVec.x = Random.Range(-1.0f, 1.0f);
                    randVec.z = Random.Range(-1.0f, 1.0f);

                    randVec.Normalize();//正規化
                    randVec *= SpawnLength;//倍化

                    //生成
                    GameObject enemyObj = Instantiate(SpawnEnemy, midPos + randVec, Quaternion.identity);

                    //レベルを設定
                    Enemy_3d enemyController = enemyObj.GetComponent<Enemy_3d>();
                    if (enemyController != null) enemyController.SetLevelStatus(EnemyLevel);

                    //リストに登録
                    EnemyList.Add(enemyObj);
                }

                //タイマー設定
                SpawnEnemyRateTimer = SpawnEnemyRateTime;
            }

            //タイマー更新
            SpawnEnemyRateTimer = Mathf.Max(0.0f, SpawnEnemyRateTimer - Time.deltaTime);
        }

    }
    //エネミーを全て削除
    public void DestroyAllEnemy()
    {
        if (EnemyList == null) return;

        // 逆順に回すのが安全（Remove しながらでも問題ない）
        for (int i = EnemyList.Count - 1; i >= 0; i--)
        {
            GameObject go = EnemyList[i];
            if (go != null)
            {
                // プレイ中は Destroy、エディタなら DestroyImmediate
                if (Application.isPlaying)
                    Object.Destroy(go);
                else
                    Object.DestroyImmediate(go);
            }
        }

        EnemyList.Clear(); // リストも空にする
    }

    //エネミーをリストに追加
    public void AddEnemyList(GameObject _enemy)
    {
        EnemyList.Add(_enemy);
    }

    //エネミーを生成する
    public GameObject GenerateEnemy(GameObject _EnemyPrefab, Vector3 _Pos)
    {
        GameObject enemy = null;
        if (_EnemyPrefab != null)
        {
            enemy = Instantiate(_EnemyPrefab);
            enemy.transform.position = new Vector3(_Pos.x, SpawnEnemyHeight, _Pos.z);

            //リストに登録
            AddEnemyList(enemy);
        }

        //エネミーを返す
        return enemy;
    }

    public GameObject GenerateEnemy(GameObject _EnemyPrefab)
    {
        return GenerateEnemy(_EnemyPrefab, Vector3.zero);
    }

    public void SetSpawnEnemyPrefab(GameObject _Enemy)
    {
        TestEnemyPrefab = _Enemy;
    }
    public void SetWaveData(WaveData_Base _data)
    {
        WaveData = _data;

        //シナリオデータだったら初期化
        if (WaveData is WaveData_Scenario)
        {
            WaveData.Init();
            return;//終了
        }
    }

    public void Exclusion_EnemyList(GameObject _Enemy)
    {
        //指定されたエネミーと同一のものをリストから外す
        if (EnemyList.Contains(_Enemy))
        {
            EnemyList.Remove(_Enemy);
        }

        //エネミーを削除
        if (_Enemy != null)
        {
            Destroy(_Enemy);
        }
    }

    public void SetEnemyLevel(int _Level)
    {
        EnemyLevel = _Level;
    }
    public GameObject GetNearestEnemy(Vector3 _Pos)
    {
        if (EnemyList == null || EnemyList.Count == 0)
            return null;

        GameObject nearest = null;
        float minDistance = float.MaxValue;

        foreach (var obj in EnemyList)
        {
            if (obj == null) continue;

            //距離を算出(平方根なし)
            float distance = (obj.transform.position - _Pos).sqrMagnitude;

            //距離を比較
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = obj;
            }
        }

        return nearest;
    }
}
