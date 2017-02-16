using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class BattleCommandSpellRequest : RequestBase
{
    public void beginRequest(long battleId, int commandSpellId, bool bt_continue = false)
    {
        base.addActionField("battlecommandspell");
        base.addField("battleId", battleId);
        base.addField("commandSpellId", commandSpellId);
        if (bt_continue)
        {
            base.addBaseField();
            base.WriteParameter();
            NetworkManager.RequestStart(this);
        }
        else
        {
            base.beginRequest();
        }
    }

    public override string getMockData() => 
        NetworkManager.getMockFile("MockBattleCommandSpellRequest");

    protected override string getParameterFileName() => 
        (Application.persistentDataPath + "/battlecommandspellrequestsave.dat");

    public override string getURL() => 
        NetworkManager.getActionUrl(false);

    public override void requestCompleted(ResponseData[] responseList)
    {
        Debug.Log("BattleCommandSpellRequest::requestCompleted>");
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.BATTLE_COMMANDSPELL, responseList);
        if (((data != null) && data.checkError()) && (data.success != null))
        {
            base.completed("ok");
        }
        else
        {
            base.completed("ng");
        }
    }
}

