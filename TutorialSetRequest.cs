using System;

public class TutorialSetRequest : RequestBase
{
    protected int FlagId;

    public void beginRequest(TutorialFlag.Id flagId)
    {
        base.addField("flagId", (int) flagId);
        base.addActionField("tutorialset");
        this.FlagId = (int) flagId;
        base.beginRequest();
    }

    public override string getMockData() => 
        string.Empty;

    public override string getURL() => 
        NetworkManager.getActionUrl(false);

    public override void requestCompleted(ResponseData[] responseList)
    {
        TopHomeRequest.clearExpirationDate();
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.TUTORIAL_SET, responseList);
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

