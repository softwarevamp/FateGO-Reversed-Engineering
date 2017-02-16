using System;

public class ClassRelationEntity : DataEntityBase
{
    public int atkClass;
    public int attackRate;
    public int defClass;

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.atkClass, ":", this.defClass };
        return string.Concat(objArray1);
    }

    public float getRate() => 
        (((float) this.attackRate) / 1000f);
}

