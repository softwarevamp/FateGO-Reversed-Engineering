using System;

public class ServantRarityEntity : DataEntityBase
{
    public int atkAdjustMax;
    public int hpAdjustMax;
    public int rarity;

    public override string getPrimarykey() => 
        (string.Empty + this.rarity);
}

