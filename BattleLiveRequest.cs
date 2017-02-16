using System;
using System.Runtime.InteropServices;

public class BattleLiveRequest : RequestBase
{
    public void beginRequest(long targetId, BattleCommandData[] selectCommond, BattleUsedSkills[] usedskills, BattleError errorinfo, string BuffInfo = "")
    {
        base.addField("targetId", targetId);
        base.addField("selectCommand", JsonManager.toJson(selectCommond));
        base.addField("BuffInfo", BuffInfo);
        base.addField("usedSkill", JsonManager.toJson(usedskills));
        if (errorinfo != null)
        {
            base.addField("battleError", JsonManager.toJson(errorinfo));
        }
        base.addActionField("battlelive");
        base.beginRequest();
    }

    public override string getMockData() => 
        NetworkManager.getMockFile("MockBattleLiveRequest");

    public override string getURL() => 
        NetworkManager.getActionUrl(false);

    public override void requestCompleted(ResponseData[] responseList)
    {
        Debug.Log("BattleLiveRequest::requestCompleted");
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.BATTLE_LIVE, responseList);
        if ((data != null) && data.checkError())
        {
            object obj2;
            if (data.success.TryGetValue("battledata", out obj2))
            {
                Debug.LogError("battledata :::: " + JsonManager.toJson(data.success["battledata"]));
                SingletonMonoBehaviour<DataManager>.Instance.SetbattleActionData(JsonManager.DeserializeArray<BattleActionData>(data.success["battledata"]));
                foreach (BattleActionData data2 in SingletonMonoBehaviour<DataManager>.Instance.GetbattleActionData())
                {
                    data2.SetListValue();
                    foreach (BattleActionData.BuffData data3 in data2.buffdatalist)
                    {
                        data3.SetProcType();
                    }
                }
            }
            base.completed("OK");
        }
    }
}

