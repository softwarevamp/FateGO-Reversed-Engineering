using System;

public class ServantEquipSetRequest : RequestBase
{
    public void beginRequest(SvtEquipInfo svtEquip)
    {
        string str = JsonManager.toJson(svtEquip);
        Debug.Log("ServantEquipSetRequest [" + str + "]");
        base.addActionField("svtequipset");
        base.addField("equipInfo", "[" + str + "]");
        base.beginRequest();
    }

    public void beginRequest(SvtEquipInfo[] svtEquipList)
    {
        string data = JsonManager.toJson(svtEquipList);
        Debug.Log("ServantEquipSetRequest " + data);
        base.addActionField("svtequipset");
        base.addField("equipInfo", data);
        base.beginRequest();
    }

    public override string getURL() => 
        NetworkManager.getActionUrl(false);

    public override void requestCompleted(ResponseData[] responseList)
    {
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.SERVANT_EQUIP_SET, responseList);
        if ((data != null) && data.checkError())
        {
            base.completed("ok");
        }
        else
        {
            base.completed("ng");
        }
    }
}

