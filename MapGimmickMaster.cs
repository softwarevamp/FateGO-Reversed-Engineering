using System;
using System.Collections.Generic;

public class MapGimmickMaster : DataMasterBase
{
    public MapGimmickMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.MAP_GIMMICK);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new MapGimmickEntity[1]);
        }
    }

    public MapGimmickEntity[] getList(int war_id)
    {
        List<MapGimmickEntity> list = new List<MapGimmickEntity>();
        for (int i = 0; i < base.list.Count; i++)
        {
            MapGimmickEntity item = base.list[i] as MapGimmickEntity;
            if ((item != null) && (item.warId == war_id))
            {
                list.Add(item);
            }
        }
        return list.ToArray();
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<MapGimmickEntity>(obj);
}

