using UnityEngine;

public class BondBeam : BondEffect
{
    [SerializeField] protected GameObject lightningBeamPrefab;
    [SerializeField] protected GameObject lightningBeam;
    public override void Init()
    {
        //‰Šú‰»Ï‚İ‚È‚çI—¹
        if (InitedFalg == true) return;

        InitedFalg = true;//‰Šú‰»Ï‚İ‚Æ‚µ‚Äƒtƒ‰ƒO‚ğture

        if (lightningBeam == null && lightningBeamPrefab != null)
        {
            lightningBeam = Instantiate(lightningBeamPrefab);
            LightningBeam lightningBeamComp=lightningBeam.GetComponent<LightningBeam>();
            if(lightningBeamComp!=null)
            {
                lightningBeamComp.SetPoint(Player1.transform, Player2.transform);
            }
        }
    }
    public override void Update_BondEffect()
    {
        Init();
    }
}
