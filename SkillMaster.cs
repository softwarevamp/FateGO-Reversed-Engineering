using System;

public class SkillMaster : DataMasterBase
{
    public SkillMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.SKILL);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new SkillEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<SkillEntity>(obj);
}

