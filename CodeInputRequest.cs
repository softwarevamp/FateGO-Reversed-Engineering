using System;

public class CodeInputRequest : RequestBase
{
    public void beginRequest(string code)
    {
        base.addField("code", code);
        base.beginRequest();
    }

    public override string getMockData() => 
        string.Empty;

    public override string getURL() => 
        (NetworkManager.getBaseUrl(false) + "code/input");

    public override void requestCompleted(ResponseData[] responseList)
    {
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.CODE_INPUT, responseList);
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

