using System;
using System.Collections.Generic;

public class UserServantLockRequest : RequestBase
{
    public void beginRequest(long id, long servantId, int isLock)
    {
        base.addField("id", id);
        base.addField("servantId", servantId);
        base.addField("isLock", isLock);
        base.addActionField("userservantlock");
        base.beginRequest();
    }

    public override string getMockData() => 
        string.Empty;

    public override string getURL() => 
        NetworkManager.getActionUrl(false);

    public override void requestCompleted(ResponseData[] responseList)
    {
        Debug.Log(">>>>>>>>>>>>>>>>>>UserServantLockRequest::requestCompleted >:" + responseList.Length);
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.USERSERVANTLOCK, responseList);
        if ((data != null) && data.checkError())
        {
            Debug.Log("responseData ok");
            Dictionary<string, object> success = data.success;
            if (success != null)
            {
                Debug.Log("successList ok");
                base.completed(JsonManager.toJson(success));
                return;
            }
        }
        base.completed("ng");
    }
}

