using System;

public class TutorialClearRequest : RequestBase
{
    public override string getMockData() => 
        string.Empty;

    public override string getURL() => 
        NetworkManager.getActionUrl(false);

    public override void requestCompleted(ResponseData[] responseList)
    {
        TopHomeRequest.clearExpirationDate();
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.TUTORIAL_CLEAR, responseList);
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

