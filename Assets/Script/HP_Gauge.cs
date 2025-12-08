using UnityEngine;
using UnityEngine.UI;

public class HP_Gauge : MonoBehaviour
{
    [SerializeField]
    private Image CurrentBar_Image;//現在バーのイメージコンポ
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetGaugeValue(float _CurrentValue, float _MaxValue)
    {
        if (CurrentBar_Image != null)
        {
            if (_MaxValue != 0.0f)//0以外で割り算
            {
                float Ratio = _CurrentValue / _MaxValue;//割合を計算
                CurrentBar_Image.fillAmount = Ratio;
            }
            else
            {
                CurrentBar_Image.fillAmount = 0.0f;
            }
        }
    }
}
