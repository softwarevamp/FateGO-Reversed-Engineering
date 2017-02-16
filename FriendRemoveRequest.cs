using System;

public class FriendRemoveRequest : RequestBase
{
    public void beginRequest(long targetUserId)
    {
        Debug.Log("FriendRemoveRequest:beginRequest [" + targetUserId + "] ");
        base.addField("targetUserId", targetUserId);
        base.addActionField("friendremove");
        base.beginRequest();
    }

    public override string getURL() => 
        NetworkManager.getActionUrl(false);

    public override void requestCompleted(ResponseData[] responseList)
    {
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.FRIEND_REMOVE, responseList);
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

