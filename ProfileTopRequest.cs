using System;

public class ProfileTopRequest : RequestBase
{
    public void beginRequest(long[] targetUserIds)
    {
        Debug.Log("ProfileTopRequest:beginRequest [" + targetUserIds + "] ");
        base.addActionField("profiletop");
        base.addField("targetUserIds", targetUserIds);
        base.beginRequest();
    }

    public void beginRequestFriendCode(string friendCode)
    {
        Debug.Log("ProfileTopRequest:beginRequest [" + friendCode + "] ");
        base.addActionField("profiletop");
        base.addField("friendCode", friendCode);
        base.beginRequest();
    }

    public override string getURL() => 
        NetworkManager.getActionUrl(false);

    public override void requestCompleted(ResponseData[] responseList)
    {
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.PROFILE, responseList);
        if ((data != null) && data.checkError())
        {
            base.completed("ok");
        }
        else
        {
            base.completed("ng");
        }
    }
}

