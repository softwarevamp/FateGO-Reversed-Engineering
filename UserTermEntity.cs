using System;

public class UserTermEntity : DataEntityBase
{
    public string detail;
    public int id;

    public override string getPrimarykey() => 
        (string.Empty + this.id);
}

