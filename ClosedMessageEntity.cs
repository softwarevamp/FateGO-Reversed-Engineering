using System;

public class ClosedMessageEntity : DataEntityBase
{
    public int id;
    public string message;

    public override string getPrimarykey() => 
        (string.Empty + this.id);
}

