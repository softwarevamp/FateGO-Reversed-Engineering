using System;

public class UserProfileEntity : DataEntityBase
{
    public string comment;
    public long userId;

    public override string getPrimarykey() => 
        (string.Empty + this.userId);
}

