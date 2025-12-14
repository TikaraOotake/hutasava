using UnityEngine;

public class LightningFlip : MonoBehaviour
{
    public LineRenderer line;
    public float interval = 0.05f;

    private float timer = 0f;
    private bool flip = false;
    private Material mat;

    void Start()
    {
        mat = line.material;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            timer = 0f;
            flip = !flip;

            Vector2 scale = mat.mainTextureScale;
            scale.x = flip ? -1f : 1f;
            mat.mainTextureScale = scale;
        }
    }
}
