using UnityEngine;

public class YAxisBillboard : MonoBehaviour
{
    [SerializeField] Camera targetCamera;
    [SerializeField] float yawOffset = 90f; // -90 にすれば逆

    void Start()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;
    }

    void LateUpdate()
    {
        if (targetCamera == null) return;

        // カメラの向き
        Vector3 forward = targetCamera.transform.forward;

        // 水平成分のみ使用
        //forward.y = 0f;
        //if (forward.sqrMagnitude < 0.0001f) return;
        //forward.Normalize();

        // カメラ forward を向かせる
        Quaternion look = Quaternion.LookRotation(forward, Vector3.up);

        // ヨー方向に90度回転
        transform.rotation = look * Quaternion.Euler(0f, yawOffset, 0f);
    }
}
