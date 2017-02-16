using System;

public class TelopEntity : DataEntityBase
{
    public int id;
    public string message;

    public override string getPrimarykey() => 
        (string.Empty + this.id);
}

