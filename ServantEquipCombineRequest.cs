using System;
using System.Collections.Generic;

public class ServantEquipCombineRequest : RequestBase
{
    public void beginRequest(long baseUsrSvtId, string materialSvtIds)
    {
        base.addActionField("svtequipcombine");
        base.addField("baseUserSvtId", baseUsrSvtId);
        base.addField("materialUserSvtIds", materialSvtIds);
        base.beginRequest();
    }

    public override string getURL() => 
        NetworkManager.getActionUrl(false);

    public override void requestCompleted(ResponseData[] responseList)
    {
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.SERVANT_EQUIP_COMBINE, responseList);
        if ((data != null) && data.checkError())
        {
            Dictionary<string, object> success = data.success;
            if (success != null)
            {
                string result = success["successResult"].ToString();
                Debug.Log("!!** ServantEquip Combine Result: " + result);
                base.completed(result);
                return;
            }
        }
        base.completed("ng");
    }
}

