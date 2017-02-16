using System;

public class ServantTreasureDvcCombineRequest : RequestBase
{
    public void beginRequest(long baseUsrSvtId, int selectTdIndex, int selectTdId, string materialSvtIds)
    {
        base.addActionField("cardcombinetd");
        base.addField("baseUserSvtId", baseUsrSvtId);
        base.addField("num", selectTdIndex);
        base.addField("treasureDeviceId", selectTdId);
        base.addField("materialUserSvtIds", materialSvtIds);
        base.beginRequest();
    }

    public override string getURL() => 
        NetworkManager.getActionUrl(false);

    public override void requestCompleted(ResponseData[] responseList)
    {
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.CARD_COMBINE_TREASUREDVC, responseList);
        if ((data != null) && data.checkError())
        {
            base.completed(JsonManager.toJson(data.success));
        }
        else
        {
            base.completed("ng");
        }
    }
}

