using System;
using System.Collections.Generic;

public class BattleResumeRequest : RequestBase
{
    public void beginRequest(long battleId, int questId, int questPhase)
    {
        base.addField("battleId", battleId);
        base.addField("questId", questId);
        base.addField("questPhase", questPhase);
        base.addActionField("battleresume");
        base.beginRequest();
    }

    public override string getMockData() => 
        NetworkManager.getMockFile("MockBattleResumeRequest");

    public override string getURL() => 
        NetworkManager.getActionUrl(false);

    public override void requestCompleted(ResponseData[] responseList)
    {
        Debug.Log("BattleResumeRequest::requestCompleted>");
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.BATTLE_RESUME, responseList);
        if ((data != null) && data.checkError())
        {
            Dictionary<string, object> success = data.success;
            if (success != null)
            {
                base.completed(JsonManager.toJson(success));
                return;
            }
        }
        base.completed("ng");
    }
}

