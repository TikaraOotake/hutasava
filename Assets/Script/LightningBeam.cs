using UnityEngine;

public class LightningBeam : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;

    public LineRenderer line;

    [Header("雷の見た目設定")]
    public int segments = 12;      // 雷の分割数
    public float noiseStrength = 0.3f; // 揺れの大きさ

    [Header("当たり判定設定")]
    public LayerMask hitMask;

    void Update()
    {
        if (!startPoint || !endPoint) return;

        DrawLightning();
        CheckHit();
    }

    void DrawLightning()
    {
        Vector3 s = startPoint.position;
        Vector3 e = endPoint.position;

        line.positionCount = segments;

        for (int i = 0; i < segments; i++)
        {
            // 0〜1の割合
            float t = i / (float)(segments - 1);

            // 基本線
            Vector3 pos = Vector3.Lerp(s, e, t);

            // 雷のジグザグ揺らし
            // 端点は揺らさない
            if (i != 0 && i != segments - 1)
            {
                pos += Random.insideUnitSphere * noiseStrength;
            }

            line.SetPosition(i, pos);
        }
    }

    void CheckHit()
    {
        Vector3 s = startPoint.position;
        Vector3 e = endPoint.position;
        Vector3 dir = (e - s).normalized;
        float dist = Vector3.Distance(s, e);

        if (Physics.Raycast(s, dir, out RaycastHit hit, dist, hitMask))
        {
            Debug.Log("Hit target: " + hit.collider.name);

            // 例：当たった相手にダメージ処理などを書く
            // hit.collider.GetComponent<Enemy>()?.Damage(1);
        }
    }

    public void SetPoint(Transform _point1, Transform _point2)
    {
        startPoint = _point1;
        endPoint = _point2;
    }
}
