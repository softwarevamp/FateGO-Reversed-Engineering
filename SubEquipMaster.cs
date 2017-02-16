using System;

public class SubEquipMaster : DataMasterBase
{
    public SubEquipMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.SUB_EQUIP);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new SubEquipEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<SubEquipEntity>(obj);
}

