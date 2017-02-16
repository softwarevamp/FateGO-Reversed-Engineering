using System;
using UnityEngine;

public class BattleUseContinueRequest : RequestBase
{
    public void beginRequest(long battleId)
    {
        base.addActionField("battleusecontinue");
        base.addField("battleId", battleId);
        base.addBaseField();
        base.WriteParameter();
        NetworkManager.RequestStart(this);
    }

    public override string getMockData() => 
        NetworkManager.getMockFile("MockBattleUseContinueRequest");

    protected override string getParameterFileName() => 
        (Application.persistentDataPath + "/battleusecontinuerequestsave.dat");

    public override string getURL() => 
        NetworkManager.getActionUrl(false);

    public override void requestCompleted(ResponseData[] responseList)
    {
        Debug.Log("BattleUseContinueRequest::requestCompleted>");
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.BATTLE_USECONTINUE, responseList);
        if ((data != null) && (data.success != null))
        {
            base.completed("ok");
        }
        else
        {
            base.completed("ng");
        }
    }
}

