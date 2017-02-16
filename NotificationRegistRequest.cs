using System;

public class NotificationRegistRequest : RequestBase
{
    public void beginRequest(string key)
    {
        base.addField("registPushKey", key);
        base.beginRequest();
    }

    public override string getMockData() => 
        string.Empty;

    public override string getURL() => 
        (NetworkManager.getBaseUrl(false) + "notification/regist");
}

