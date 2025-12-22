using NUnit.Framework.Interfaces;
using System;
using UnityEngine;
using static UnityEditor.Progress;

public class UI_RewardSlot_V2 : UI_Base
{
    new public event Action<UI_Base> OnSelected;
    public void Decide()
    {
        OnSelected?.Invoke(this); // Å©Ç±Ç±Ç≈åƒÇŒÇÍÇÈ
    }
}
