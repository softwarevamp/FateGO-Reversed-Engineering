using System;
using System.Collections.Generic;

public class SpotRoadMaster : DataMasterBase
{
    public SpotRoadMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.SPOT_ROAD);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new SpotRoadEntity[1]);
        }
    }

    public SpotRoadEntity[] getList(int war_id)
    {
        List<SpotRoadEntity> list = new List<SpotRoadEntity>();
        for (int i = 0; i < base.list.Count; i++)
        {
            SpotRoadEntity item = base.list[i] as SpotRoadEntity;
            if ((item != null) && (item.getWarId() == war_id))
            {
                list.Add(item);
            }
        }
        return list.ToArray();
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<SpotRoadEntity>(obj);
}

