using System;

public class TipsEntity : DataEntityBase
{
    public string comment;
    public int id;

    public override string getPrimarykey() => 
        (string.Empty + this.id);
}

