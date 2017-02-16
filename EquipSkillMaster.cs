using System;

public class EquipSkillMaster : DataMasterBase
{
    public EquipSkillMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.EQUIP_SKILL);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new EquipSkillEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<EquipSkillEntity>(obj);
}

