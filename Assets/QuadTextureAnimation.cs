using System.Collections.Generic;
using UnityEngine;

public class QuadTextureAnimation : MonoBehaviour
{
    [SerializeField] private List<Texture> textures = new List<Texture>();
    [SerializeField] private float frameRate = 10f;
    [SerializeField] private bool flipX;

    static readonly int MainTexId = Shader.PropertyToID("_MainTex");
    static readonly int MainTexSTId = Shader.PropertyToID("_MainTex_ST");

    private MeshRenderer mr;
    private MaterialPropertyBlock mpb;
    private int index;
    private float timer;

    void Awake()
    {
        Init();
        ApplyFirstTexture();
    }

    void OnValidate()
    {
        Init();
        ApplyFirstTexture();
    }

    //初期化
    void Init()
    {
        if (mr == null)
            mr = GetComponent<MeshRenderer>();

        if (mpb == null)
            mpb = new MaterialPropertyBlock();
    }

    //リスト内の最初の一枚を即座に反映する
    void ApplyFirstTexture()
    {
        if (mr == null) return;
        if (textures == null || textures.Count == 0) return;

        index = 0;
        timer = 0f;

        ApplyTexture(textures[0]);
    }

    //Texture適用
    void ApplyTexture(Texture tex)
    {
        if (tex == null) return;

        mr.GetPropertyBlock(mpb);

        mpb.SetTexture(MainTexId, tex);

        // 左右反転設定
        float scaleX = flipX ? -1f : 1f;
        float offsetX = flipX ? 1f : 0f;
        mpb.SetVector(MainTexSTId, new Vector4(scaleX, 1f, offsetX, 0f));

        mr.SetPropertyBlock(mpb);
    }

    void Update()
    {
        if (textures == null || textures.Count == 0) return;

        timer += Time.deltaTime;
        if (timer >= 1f / frameRate)
        {
            timer = 0f;
            index = (index + 1) % textures.Count;
            ApplyTexture(textures[index]);
        }
    }

    public void SetFlipX(bool _flag)
    {
        flipX = _flag;
    }
    public void SetTexture(List<Texture> _list)
    {
        textures = _list;
    }
}
