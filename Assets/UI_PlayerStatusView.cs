using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class UI_PlayerStatusView : MonoBehaviour
{
    [SerializeField] private Image PlayerImage;
    [SerializeField] private Text HP_MaxText;
    [SerializeField] private Text AttackText;
    [SerializeField] private Text SpeedText;

    private string Label_HP;
    private string Label_Atk;
    private string Label_Speed;

    private void Awake()
    {
        if (HP_MaxText != null) Label_HP = HP_MaxText.text;
        if (AttackText != null) Label_Atk = AttackText.text;
        if (SpeedText != null) Label_Speed = SpeedText.text;

        //ï\é¶Ç0Ç≈èâä˙âª
        //SetPlayerStatus(0, 0, 0);
    }
    public void SetPlayerStatus(int _HP, int _Atk, int _Speed)
    {
        if (HP_MaxText != null) HP_MaxText.text = Label_HP + _HP.ToString();
        if (AttackText != null) AttackText.text = Label_Atk + _Atk.ToString();
        if (SpeedText != null) SpeedText.text = Label_Speed + _Speed.ToString();
    }
}
