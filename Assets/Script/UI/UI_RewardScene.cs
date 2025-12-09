using UnityEngine;
using UnityEngine.UI;
public class UI_RewardScene : MonoBehaviour
{
    [SerializeField] private Image Player1ItemImage;
    [SerializeField] private Image Player2ItemImage;

    [SerializeField] private UI_SelectSlot[] RewardSelectImageArrey = new UI_SelectSlot[5];

    [SerializeField] private int Index;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int inputNum = 0;
        if (Input.GetKeyDown(KeyCode.W))
        {
            ++inputNum;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            --inputNum;
        }

        Index += inputNum;

        if (Index < 0)
        {
            Index = 0;
        }
        else if (Index >= RewardSelectImageArrey.Length)
        {
            Index = RewardSelectImageArrey.Length;
        }

        //‘I‘ð’†‚Ì‘I‘ðŽˆ‚ð”­Œõ-------------------

        //Žæ“¾
        UI_SelectSlot SelectSlot = RewardSelectImageArrey[Index];

        if (SelectSlot != null)
        {
            SelectSlot.SetSelectingFlag();
        }
        //-----------------------------------
    }
}
