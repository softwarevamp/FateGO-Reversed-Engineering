using System;

public class SkillDetailMaster : DataMasterBase
{
    public SkillDetailMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.SKILL_DETAIL);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new SkillDetailEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<SkillDetailEntity>(obj);
}

