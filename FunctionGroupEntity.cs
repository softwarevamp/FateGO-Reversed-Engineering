using System;

public class FunctionGroupEntity : DataEntityBase
{
    public int baseFuncId;
    public int funcId;
    public bool isDispValue;
    public string name;
    public string nameTotal;
    public int priority;

    public override string getPrimarykey() => 
        (string.Empty + this.funcId);
}

