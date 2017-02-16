using System;

public class BoxGachaResetRequest : RequestBase
{
    public void beginRequest(int gachaId)
    {
        base.addActionField("boxgachareset");
        base.addField("boxGachaId", gachaId);
        base.beginRequest();
    }

    public override string getURL() => 
        NetworkManager.getActionUrl(false);

    public override void requestCompleted(ResponseData[] responseList)
    {
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.BOX_GACHA_RESET, responseList);
        Debug.Log("-*-* BoxGachaDrawRequest : " + data.success);
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

