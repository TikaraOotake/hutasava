using UnityEngine;
[RequireComponent(typeof(MeshRenderer))]
public class QuadTextureSetter : MonoBehaviour
{
    [SerializeField] Texture mainTexture;

    void Start()
    {
        if (mainTexture != null)
            SetTexture(mainTexture);
    }

    public void SetTexture(Texture tex)
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();

        // インスタンス化されたマテリアルを取得
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        mr.GetPropertyBlock(mpb);

        // _MainTex は Unlit/Texture の標準名
        mpb.SetTexture("_MainTex", tex);

        mr.SetPropertyBlock(mpb);
    }
}