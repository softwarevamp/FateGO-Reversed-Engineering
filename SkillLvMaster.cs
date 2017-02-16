using System;

public class SkillLvMaster : DataMasterBase
{
    public SkillLvMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.SKILL_LEVEL);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new SkillLvEntity[1]);
        }
    }

    public SkillLvEntity getEntityFromId(int skillId, int lv)
    {
        object[] objArray1 = new object[] { string.Empty, skillId, ":", lv };
        string key = string.Concat(objArray1);
        if (base.lookup.ContainsKey(key))
        {
            return (base.lookup[key] as SkillLvEntity);
        }
        return null;
    }

    public FunctionEntity GetEventPointFunc(int eventId)
    {
        if (eventId > 0)
        {
            int count = base.list.Count;
            for (int i = 0; i < count; i++)
            {
                SkillLvEntity entity = base.list[i] as SkillLvEntity;
                if (entity != null)
                {
                    FunctionEntity eventPointFunc = entity.GetEventPointFunc(eventId);
                    if (eventPointFunc != null)
                    {
                        return eventPointFunc;
                    }
                }
            }
        }
        return null;
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<SkillLvEntity>(obj);
}

