using System;

public class ConstantStrEntity : DataEntityBase
{
    public string name;
    public string value;

    public override string getPrimarykey() => 
        this.name;
}

