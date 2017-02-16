using System;

public class ServantExpEntity : DataEntityBase
{
    public int curve;
    public int exp;
    public int lv;
    public int type;

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.type, ":", this.lv };
        return string.Concat(objArray1);
    }
}

