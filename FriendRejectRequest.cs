using System;

public class FriendRejectRequest : RequestBase
{
    public void beginRequest(long targetUserId)
    {
        Debug.Log("FriendRejectRequest:beginRequest [" + targetUserId + "] ");
        base.addField("targetUserId", targetUserId);
        base.addActionField("friendreject");
        base.beginRequest();
    }

    public override string getURL() => 
        NetworkManager.getActionUrl(false);

    public override void requestCompleted(ResponseData[] responseList)
    {
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.FRIEND_REJECT, responseList);
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

