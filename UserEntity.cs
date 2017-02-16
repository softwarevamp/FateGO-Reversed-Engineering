using System;

public class UserEntity : DataEntityBase
{
    public string authKey;
    public int deviceType;
    public long id;
    public int isDel;
    public string pushKey;
    public string secretKey;
    public string userAgent;

    public override string getPrimarykey() => 
        (string.Empty + this.id);
}

