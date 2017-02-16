using System;

public class ShopScriptEntity : DataEntityBase
{
    public int eventId;
    public string name;
    public int priority;
    public string scriptId;
    public int shopId;
    public int svtId;

    public override string getPrimarykey() => 
        (string.Empty + this.shopId);
}

