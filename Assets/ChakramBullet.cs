using UnityEngine;

public class ChakramBullet : PlayerBullet
{
    [SerializeField] private float RotateLength = 1.0f;//回転半径
    [SerializeField] private float RotateSpeed = 1.0f;//回転速度 


    // Update is called once per frame
    void Update()
    {
        if (Player != null)
        {
            //プレイヤー座標取得
            Vector3 pos = Player.transform.position;

            //角度
            float Angle = Time.time * RotateSpeed;

            //回転したベクトルを計算
            Vector2 rotateVec = new Vector2(Mathf.Sin(Angle), Mathf.Cos(Angle)) * RotateLength;

            //座標代入
            transform.position = pos + new Vector3(rotateVec.x, 0.0f, rotateVec.y);
        }

    }
}
