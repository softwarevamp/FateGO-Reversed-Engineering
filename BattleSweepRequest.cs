using System;
using System.Collections.Generic;

public class BattleSweepRequest : RequestBase
{
    public static bool test;

    public void beginRequest(int questId, int questPhase, long activeDeckId, long followerId, int battleType)
    {
        test = true;
        base.addField("questId", questId);
        base.addField("questPhase", questPhase);
        base.addField("activeDeckId", activeDeckId);
        base.addField("followerId", followerId);
        base.addField("battleMode", battleType);
        base.addActionField("battlesweep");
        base.beginRequest();
    }

    public override string getMockData() => 
        NetworkManager.getMockFile("MockBattleSetupRequest");

    public override string getURL() => 
        NetworkManager.getActionUrl(false);

    public override void requestCompleted(ResponseData[] responseList)
    {
        Debug.Log("BattleSetupRequest::requestCompleted>");
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.BATTLE_RESULT, responseList);
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

