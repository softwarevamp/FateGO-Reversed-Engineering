using System;

public class EventMissionClearRewardRequest : RequestBase
{
    public void beginRequest(int missionId)
    {
        base.addActionField("eventmissionreceive");
        base.addField("missionId", missionId);
        base.beginRequest();
    }

    public override string getURL() => 
        NetworkManager.getActionUrl(false);

    public override void requestCompleted(ResponseData[] responseList)
    {
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.EV_MISSION_REWARD, responseList);
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

