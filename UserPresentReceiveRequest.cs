using System;

public class UserPresentReceiveRequest : RequestBase
{
    public void beginRequest(long[] presentIds)
    {
        base.addField("presentIds", presentIds);
        base.beginRequest();
    }

    public override string getMockData() => 
        NetworkManager.getMockFile("MockUserPresentReceiveResponse");

    public override string getURL() => 
        NetworkManager.getActionUrl(false);

    public override void requestCompleted(ResponseData[] responseList)
    {
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.PRESENT_RECEIVE, responseList);
        if (((data != null) && data.checkError()) && (data.success != null))
        {
            base.completed(JsonManager.toJson(data.success));
        }
        else
        {
            base.completed("ng");
        }
    }
}

