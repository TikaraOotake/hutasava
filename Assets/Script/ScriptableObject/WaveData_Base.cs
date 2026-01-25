
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "ScriptableObjects/WaveData_Base")]
public class WaveData_Base :  ScriptableObject
{
    struct EnemyDataContainer
    {
        public GameObject SpawnEnemy;
        public float LevelModifierRatio;//Level補正倍率
        public float SpawnRate;//出現率
    }


    //[SerializeField] private List<EnemyDataContainer> EnemyDataList = new List<EnemyDataContainer>();//エネミーの情報リスト

    [SerializeField] private List<GameObject> EnemyPrefabList = new List<GameObject>();//エネミーのプレハブリスト
    [SerializeField] private List<float> LevelModifierRatioList = new List<float>();//エネミーのLevel補正リスト
    [SerializeField] private List<float> SpawnRateList = new List<float>();//エネミーの出現率リスト

    [SerializeField] protected List<Vector2> SpawnPosList = new List<Vector2>();//エネミーの出現座標リスト

    [SerializeField] private int TreasureLevel;//報酬Level
    [SerializeField] private int DestroyQuota;//撃破ノルマ

    [SerializeField] private GameObject BossEnemyPrefab;//ボス敵のプレハブ

    //リスト同期用変数
    private int prevA, prevB, prevC;

    public virtual void Init()
    {

    }
    public virtual void Update_Wave()
    {

    }

    public List<GameObject> GetEnemyPrefabList()
    {
        return EnemyPrefabList;
    }
    public List<float> GetLevelModifierRatioList()
    {
        return LevelModifierRatioList;
    }
    public List<float> GetSpawnRateList()
    {
        return SpawnRateList;
    }

    //設定された割合を元にランダムにエネミープレハブを取得する
    public GameObject GetSpawnEnemyRand()
    {
        return EnemyPrefabList[GetSpawnEnemyIndex()];
    }

    //出現比重リストを基準にランダムにインデックスを選ぶ
    public int GetSpawnEnemyIndex()
    {
        return GetWeightedIndex(SpawnRateList);
    }


    public int GetWeightedIndex(List<float> weights)
    {
        // 合計重みを計算
        float total = 0f;
        foreach (float w in weights)
            total += w;

        // 0 ~ total の間でランダム取得
        float r = Random.Range(0f, total);

        // どの重み範囲に入るかを判定
        float cumulative = 0f;
        for (int i = 0; i < weights.Count; i++)
        {
            cumulative += weights[i];
            if (r <= cumulative)
                return i;
        }

        return weights.Count - 1; // 念のため（浮動小数誤差対策）
    }

    protected void OnValidate()
    {
        //リストサイズ同期用処理

        // どれが変更されたか判定
        bool aChanged = EnemyPrefabList.Count != prevA;
        bool bChanged = LevelModifierRatioList.Count != prevB;
        bool cChanged = SpawnRateList.Count != prevC;

        // 変更されたリストのサイズを基準にする
        int targetSize = -1;

        if (aChanged) targetSize = EnemyPrefabList.Count;
        else if (bChanged) targetSize = LevelModifierRatioList.Count;
        else if (cChanged) targetSize = SpawnRateList.Count;
        else return; // 何も変わってなければ終了

        // サイズ同期
        SyncSize(EnemyPrefabList, targetSize);
        SyncSize(LevelModifierRatioList, targetSize);
        SyncSize(SpawnRateList, targetSize);

        // サイズを更新して次回比較に使う
        prevA = EnemyPrefabList.Count;
        prevB = LevelModifierRatioList.Count;
        prevC = SpawnRateList.Count;
    }

    protected void SyncSize<T>(List<T> list, int size)
    {
        if (list.Count == size) return;

        if (list.Count < size)
        {
            while (list.Count < size)
                list.Add(default);
        }
        else
        {
            list.RemoveRange(size, list.Count - size);
        }
    }
    protected void SyncSize<T>(List<T> list, int size, T initialValue)
    {
        if (list.Count == size) return;

        if (list.Count < size)
        {
            while (list.Count < size)
                list.Add(initialValue);
        }
        else
        {
            list.RemoveRange(size, list.Count - size);
        }
    }
}
