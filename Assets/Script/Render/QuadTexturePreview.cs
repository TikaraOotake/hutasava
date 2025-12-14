using UnityEngine;
[ExecuteAlways]
[RequireComponent(typeof(MeshRenderer))]
public class QuadTexturePreview : MonoBehaviour
{
    [SerializeField] Texture mainTexture;

    static readonly int MainTexId = Shader.PropertyToID("_MainTex");

    void OnValidate()
    {
        Apply();
    }

    void Awake()
    {
        Apply();
    }

    void Apply()
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        if (mr == null || mainTexture == null) return;

        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        mr.GetPropertyBlock(mpb);
        mpb.SetTexture(MainTexId, mainTexture);
        mr.SetPropertyBlock(mpb);
    }
}