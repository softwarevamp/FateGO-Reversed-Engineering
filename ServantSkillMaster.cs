using System;
using System.Collections.Generic;

public class ServantSkillMaster : DataMasterBase
{
    public ServantSkillMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.SERVANT_SKILL);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new ServantSkillEntity[1]);
        }
    }

    public ServantSkillEntity getEntityFromId(int svtId, int num, int priority)
    {
        object[] objArray1 = new object[] { string.Empty, svtId, ":", num, ":", priority };
        string key = string.Concat(objArray1);
        if (base.lookup.ContainsKey(key))
        {
            return (base.lookup[key] as ServantSkillEntity);
        }
        return null;
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<ServantSkillEntity>(obj);

    public ServantSkillEntity[] getServantSkillList(int svtId)
    {
        int count = base.list.Count;
        List<ServantSkillEntity> list = new List<ServantSkillEntity>();
        for (int i = 0; i < count; i++)
        {
            ServantSkillEntity item = base.list[i] as ServantSkillEntity;
            if ((item != null) && (item.svtId == svtId))
            {
                list.Add(item);
            }
        }
        return list.ToArray();
    }
}

