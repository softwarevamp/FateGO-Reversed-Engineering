using System;

public class FriendCancelRequest : RequestBase
{
    public void beginRequest(long targetUserId)
    {
        Debug.Log("FriendCancelRequest:beginRequest [" + targetUserId + "] ");
        base.addField("targetUserId", targetUserId);
        base.beginRequest();
    }

    public override string getURL() => 
        (NetworkManager.getBaseUrl(false) + "friend/cancel");

    public override void requestCompleted(ResponseData[] responseList)
    {
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.FRIEND_CANCEL, responseList);
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

