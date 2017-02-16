using System;

public class ContinueInputRequest : RequestBase
{
    public void beginRequest(string continueKey, string continuePass)
    {
        base.addField("continueKey", continueKey);
        base.addField("continuePass", continuePass);
        base.beginRequest();
    }

    public override string getMockData() => 
        string.Empty;

    public override string getURL() => 
        (NetworkManager.getBaseUrl(true) + "continue/input");

    public override void requestCompleted(ResponseData[] responseList)
    {
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.CONTINUE_INPUT, responseList);
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

