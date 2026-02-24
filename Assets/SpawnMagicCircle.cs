using UnityEngine;

public class SpawnMagicCircle : MonoBehaviour
{
    [SerializeField] private float ScalingSpeed;//拡縮速度
    [SerializeField] private float RotateSpeed;//回転速度

    private Vector3 BaseSize;//基準となる大きさ

    private Vector3 TargetSize;//目標となる大きさ

    [SerializeField] private float ActiveTimer = 0.0f;//有効時間タイマー
    [SerializeField] private float DeleteTimer = 0.0f;//削除タイマー

    [SerializeField] private GameObject Enemy;//敵


    private void Start()
    {
        //大きさ取得
        BaseSize = TargetSize = transform.localScale;


        //大きさを最小化
        transform.localScale = Vector3.zero;

        //座標をエネミーに合わせる
        if (Enemy != null)
        {
            transform.position = Enemy.transform.position;
        }
    }

    private void Update()
    {
        //徐々に大きさを目標サイズに寄せる
        Vector3 Size = Vector3.Lerp(transform.localScale, TargetSize, 1 - Mathf.Exp(-1.0f * ScalingSpeed * Time.deltaTime));

        //代入
        transform.localScale = Size;

        //回転する
        Vector3 angle = transform.eulerAngles;
        angle.y += RotateSpeed * Time.deltaTime;
        transform.eulerAngles = angle;

        if (ActiveTimer <= 0.0f)
        {
            //目標サイズを0に指定
            TargetSize = Vector3.zero;

            //
            if (Enemy != null)
            {
                Enemy.SetActive(true);
            }

            if (DeleteTimer <= 0.0f)
            {
                Destroy(this.gameObject);//自身を削除
            }

            //タイマー更新
            DeleteTimer = Mathf.Max(0.0f, DeleteTimer - Time.deltaTime);
        }

        ActiveTimer = Mathf.Max(0.0f, ActiveTimer - Time.deltaTime);//タイマー更新
    }

    public void SetEnemy(GameObject _obj)
    {
        Enemy = _obj;
    }
}
