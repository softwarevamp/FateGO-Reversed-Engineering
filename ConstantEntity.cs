using System;

public class ConstantEntity : DataEntityBase
{
    public string name;
    public int value;

    public override string getPrimarykey() => 
        this.name;
}

