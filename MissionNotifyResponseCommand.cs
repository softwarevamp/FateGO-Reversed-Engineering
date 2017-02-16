using System;
using System.Collections.Generic;

public class MissionNotifyResponseCommand : ResponseCommandBase
{
    private const string RESPONSE_KEY = "eventMissionAnnounce";

    public override ResponseCommandBase.Result ExecuteResponse(ResponseData data)
    {
        Dictionary<string, object> success = data.success;
        if ((success != null) && success.ContainsKey("eventMissionAnnounce"))
        {
            object obj2 = success["eventMissionAnnounce"];
            foreach (MissionNotifyDispInfo info in JsonManager.DeserializeArray<MissionNotifyDispInfo>(obj2))
            {
                SingletonTemplate<MissionNotifyManager>.Instance.RequestDisp(info);
            }
        }
        return ResponseCommandBase.Result.SUCCESS;
    }

    public override ResponseCommandKind.Kind GetKind() => 
        ResponseCommandKind.Kind.MISSION_NOTIFY;
}

