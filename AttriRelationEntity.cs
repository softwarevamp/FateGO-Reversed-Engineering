using System;

public class AttriRelationEntity : DataEntityBase
{
    public int atkAttri;
    public int attackRate;
    public int defAttri;

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.atkAttri, ":", this.defAttri };
        return string.Concat(objArray1);
    }

    public float getRate() => 
        (((float) this.attackRate) / 1000f);
}

