using System;

public class AnnouncementRequest : RequestBase
{
    public void beginRequest()
    {
        base.addActionField("announcementlist");
        base.beginRequest();
    }

    public override string getMockData() => 
        NetworkManager.getMockFile("AnnouncementRequest");

    public override string getURL() => 
        NetworkManager.getActionUrl(false);

    public override void requestCompleted(ResponseData[] responseList)
    {
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.ANNOUNCEMENTLIST, responseList);
        if ((data != null) && data.checkError())
        {
            base.completed(JsonManager.toJson(data.success));
        }
    }
}

