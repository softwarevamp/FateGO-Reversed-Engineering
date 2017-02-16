using System;

public class UserSuperBossEntity : DataEntityBase
{
    public long damage;
    public int eventId;
    public int superBossId;
    public long userId;

    public override string getPrimarykey()
    {
        string[] textArray1 = new string[] { this.userId.ToString(), ":", this.eventId.ToString(), ":", this.superBossId.ToString() };
        return string.Concat(textArray1);
    }
}

