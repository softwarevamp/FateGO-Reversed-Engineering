using System;

public class CvEntity : DataEntityBase
{
    public string comment;
    public int id;
    public string name;

    public override string getPrimarykey() => 
        (string.Empty + this.id);
}

