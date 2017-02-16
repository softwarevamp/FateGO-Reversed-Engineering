using System;
using System.Collections.Generic;

public class BattleSetupRequest : RequestBase
{
    public void beginRequest(int questId, int questPhase, long activeDeckId, long followerId, int followerClassId, int battleResultType)
    {
        base.addField("questId", questId);
        base.addField("questPhase", questPhase);
        base.addField("activeDeckId", activeDeckId);
        base.addField("followerId", followerId);
        base.addField("followerClassId", followerClassId);
        base.addField("battleMode", battleResultType);
        base.addActionField("battlesetup");
        BattleData.setReserveResumeBattle(true, questId, questPhase);
        base.beginRequest();
    }

    public override string getMockData() => 
        NetworkManager.getMockFile("MockBattleSetupRequest");

    public override string getURL() => 
        NetworkManager.getActionUrl(false);

    public override void requestCompleted(ResponseData[] responseList)
    {
        Debug.Log("BattleSetupRequest::requestCompleted>");
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.BATTLE_SETUP, responseList);
        if ((data != null) && data.checkError())
        {
            Dictionary<string, object> success = data.success;
            if (success != null)
            {
                base.completed(JsonManager.toJson(success));
                BattleData.setReserveResumeBattle(false, 0, 0);
                return;
            }
        }
        base.completed("ng");
    }
}

