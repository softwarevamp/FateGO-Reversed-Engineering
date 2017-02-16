using System;

public class UserFormationEntity : DataEntityBase
{
    public int[] itemId;
    public long userEquipId;
    public long userId;
    public long userSubEquipId;

    public override string getPrimarykey() => 
        (string.Empty + this.userId);
}

