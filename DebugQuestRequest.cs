using System;

public class DebugQuestRequest : RequestBase
{
    public void beginRequest(bool is_enable)
    {
        base.addField("flag", !is_enable ? 0 : 1);
        base.beginRequest();
    }

    public override string getMockData() => 
        string.Empty;

    public override string getURL() => 
        (NetworkManager.getBaseUrl(false) + "ClientDebug/QuestThrough");

    public override void requestCompleted(ResponseData[] responseList)
    {
        TopHomeRequest.clearExpirationDate();
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.DEBUG_QUEST, responseList);
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

