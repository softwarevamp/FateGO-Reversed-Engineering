using System;

public class TreasureDvcDetailEntity : DataEntityBase
{
    public string detail;
    public string detailShort;
    public int id;

    public override string getPrimarykey() => 
        (string.Empty + this.id);
}

