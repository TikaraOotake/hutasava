using UnityEngine;

public class BondEffect_Base : ScriptableObject
{
    private int Level = 0;
    public virtual void Update_BondEffect()
    {
        return;
    }
    public void SetLevel(int _Level)
    {
        Level = _Level;
    }
}
