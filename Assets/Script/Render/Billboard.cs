using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] Camera targetCamera;

    void Start()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;
    }

    void LateUpdate()
    {
        if (targetCamera == null) return;

        // カメラ → オブジェクト方向ベクトル
        Vector3 dir = transform.position - targetCamera.transform.position;

        if (dir.sqrMagnitude < 0.0001f) return;

        // 向きを合わせる
        transform.rotation = Quaternion.LookRotation(dir);
    }
}
