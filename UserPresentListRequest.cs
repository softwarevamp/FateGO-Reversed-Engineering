using System;

public class UserPresentListRequest : RequestBase
{
    public void beginRequest()
    {
        base.addActionField("presentlist");
        base.beginRequest();
    }

    public override string getMockData() => 
        NetworkManager.getMockFile("MockUserPresentListResponse");

    public override string getURL() => 
        NetworkManager.getActionUrl(false);
}

