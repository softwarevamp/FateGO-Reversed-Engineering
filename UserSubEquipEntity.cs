using System;

public class UserSubEquipEntity : DataEntityBase
{
    public long id;
    public int subEquipId;
    public long userId;

    public override string getPrimarykey() => 
        (string.Empty + this.id);
}

