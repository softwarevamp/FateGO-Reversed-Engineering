using System;

public class BoxGachaExtraEntity : DataEntityBase
{
    public string detail;
    public int iconId;
    public int id;
    public string name;

    public override string getPrimarykey() => 
        (string.Empty + this.id);
}

