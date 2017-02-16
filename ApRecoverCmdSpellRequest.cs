using System;

public class ApRecoverCmdSpellRequest : RequestBase
{
    public void beginRequest(int itemId)
    {
        base.addField("commandSpellId", itemId);
        base.beginRequest();
    }

    public override string getURL() => 
        (NetworkManager.getBaseUrl(false) + "commandSpell/use");

    public override void requestCompleted(ResponseData[] responseList)
    {
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.COMMANDSPELL_USE, responseList);
        if ((data != null) && data.checkError())
        {
            string result = "ok";
            base.completed(result);
        }
        else
        {
            base.completed("ng");
        }
    }
}

