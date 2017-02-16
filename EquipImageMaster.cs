using System;

public class EquipImageMaster : DataMasterBase
{
    public EquipImageMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.EQUIP_IMAGE);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new EquipImageEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<EquipImageEntity>(obj);
}

