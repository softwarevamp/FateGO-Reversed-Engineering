using System;

public class UserContinueEntity : DataEntityBase
{
    public string continueKey;
    public int isDel;
    public long remakeAt;
    public long userId;

    public override string getPrimarykey() => 
        (string.Empty + this.userId);
}

