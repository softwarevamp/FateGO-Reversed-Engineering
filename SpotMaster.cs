using System;
using System.Collections.Generic;

public class SpotMaster : DataMasterBase
{
    public SpotMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.SPOT);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new SpotEntity[1]);
        }
    }

    public SpotEntity[] getList(int war_id)
    {
        List<SpotEntity> list = new List<SpotEntity>();
        for (int i = 0; i < base.list.Count; i++)
        {
            SpotEntity item = base.list[i] as SpotEntity;
            if ((item != null) && (item.getWarId() == war_id))
            {
                list.Add(item);
            }
        }
        return list.ToArray();
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<SpotEntity>(obj);
}

