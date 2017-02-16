using System;

public class CombineQpSvtEquipMaster : DataMasterBase
{
    public CombineQpSvtEquipMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.COMBINE_QP_SVT_EQUIP);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new CombineQpSvtEquipEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<CombineQpSvtEquipEntity>(obj);
}

