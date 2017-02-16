using System;

public class BoxGachaDrawRequest : RequestBase
{
    public void beginRequest(int gachaId, int num)
    {
        base.addActionField("boxgachadraw");
        base.addField("boxGachaId", gachaId);
        base.addField("num", num);
        base.beginRequest();
    }

    public override string getURL() => 
        NetworkManager.getActionUrl(false);

    public override void requestCompleted(ResponseData[] responseList)
    {
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.BOX_GACHA_DRAW, responseList);
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

