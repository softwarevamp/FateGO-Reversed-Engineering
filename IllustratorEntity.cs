using System;

public class IllustratorEntity : DataEntityBase
{
    public string comment;
    public int id;
    public string name;

    public override string getPrimarykey() => 
        (string.Empty + this.id);
}

