using System;

public class UserLoginEntity : DataEntityBase
{
    public long lastLoginAt;
    public int seqLoginCount;
    public int totalLoginCount;
    public long userId;

    public override string getPrimarykey() => 
        (string.Empty + this.userId);
}

