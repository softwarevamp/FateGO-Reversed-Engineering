using System;

public class FriendAcceptRequest : RequestBase
{
    public void beginRequest(long targetUserId)
    {
        Debug.Log("FriendAcceptRequest:beginRequest [" + targetUserId + "] ");
        base.addField("targetUserId", targetUserId);
        base.addActionField("friendaccept");
        base.beginRequest();
    }

    public override string getURL() => 
        NetworkManager.getActionUrl(false);

    public override void requestCompleted(ResponseData[] responseList)
    {
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.FRIEND_ACCEPT, responseList);
        if ((data != null) && data.checkError())
        {
            base.completed(JsonManager.toJson(data.success));
        }
        else
        {
            base.completed("ng");
        }
    }
}

