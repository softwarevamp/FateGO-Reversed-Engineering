using System;

public class CombineSkillMaster : DataMasterBase
{
    public CombineSkillMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.COMBINE_SKILL);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new CombineSkillEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<CombineSkillEntity>(obj);
}

