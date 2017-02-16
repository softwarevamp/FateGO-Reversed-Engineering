using System;

public class ContinueDecideRequest : RequestBase
{
    public void beginRequestCode(string continueKey, string continuePass)
    {
        base.addField("continueType", 1);
        base.addField("continueKey", continueKey);
        base.addField("continuePass", continuePass);
        base.beginRequest();
    }

    public void beginRequestFacebook(string facebookId, string accessToken)
    {
        base.addField("continueType", 3);
        base.addField("facebookId", facebookId);
        base.addField("accessToken", accessToken);
        base.beginRequest();
    }

    public void beginRequestTwitter(string twitterId, string accessToken)
    {
        base.addField("continueType", 2);
        base.addField("twitterId", twitterId);
        base.addField("accessToken", accessToken);
        base.beginRequest();
    }

    public override string getMockData() => 
        string.Empty;

    public override string getURL() => 
        (NetworkManager.getBaseUrl(true) + "continue/decide");

    public override void requestCompleted(ResponseData[] responseList)
    {
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.CONTINUE_DECIDE, responseList);
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

