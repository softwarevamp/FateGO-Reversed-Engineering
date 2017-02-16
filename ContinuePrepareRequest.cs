using System;

public class ContinuePrepareRequest : RequestBase
{
    public void beginRequest(string continuePass)
    {
        base.addActionField("cdkexchange");
        base.addField("codeKey", continuePass);
        base.beginRequest();
    }

    public override string getMockData() => 
        string.Empty;

    public override string getURL() => 
        NetworkManager.getActionUrl(false);

    public override void requestCompleted(ResponseData[] responseList)
    {
        base.completed("ok");
    }
}

