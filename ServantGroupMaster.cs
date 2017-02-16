using System;
using System.Collections.Generic;

public class ServantGroupMaster : DataMasterBase
{
    public ServantGroupMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.SERVANT_GROUP);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new ServantGroupEntity[1]);
        }
    }

    public ServantGroupEntity[] getEntityListById(int groupId)
    {
        List<ServantGroupEntity> list = new List<ServantGroupEntity>();
        int count = base.list.Count;
        for (int i = 0; i < count; i++)
        {
            ServantGroupEntity item = base.list[i] as ServantGroupEntity;
            if ((item != null) && (item.id == groupId))
            {
                list.Add(item);
            }
        }
        return list.ToArray();
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<ServantGroupEntity>(obj);

    public ServantGroupEntity[] getListByServantID(int svt_id)
    {
        List<ServantGroupEntity> list = new List<ServantGroupEntity>();
        for (int i = 0; i < base.list.Count; i++)
        {
            ServantGroupEntity item = base.list[i] as ServantGroupEntity;
            if ((item != null) && (item.getServantId() == svt_id))
            {
                list.Add(item);
            }
        }
        return list.ToArray();
    }
}

