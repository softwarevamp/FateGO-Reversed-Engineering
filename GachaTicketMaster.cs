using System;

public class GachaTicketMaster : DataMasterBase
{
    public GachaTicketMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.GACHA_TICKET);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new GachaTicketEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<GachaTicketEntity>(obj);
}

