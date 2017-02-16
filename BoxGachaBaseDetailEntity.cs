using System;

public class BoxGachaBaseDetailEntity : DataEntityBase
{
    public int baseId;
    public string detailUrl;

    public override string getPrimarykey() => 
        (string.Empty + this.baseId);
}

