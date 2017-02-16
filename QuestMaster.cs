using System;
using System.Collections.Generic;

public class QuestMaster : DataMasterBase
{
    public QuestMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.QUEST);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new QuestEntity[1]);
        }
    }

    public QuestEntity[] getList(int spot_id)
    {
        List<QuestEntity> list = new List<QuestEntity>();
        for (int i = 0; i < base.list.Count; i++)
        {
            QuestEntity item = base.list[i] as QuestEntity;
            if ((item != null) && (item.spotId == spot_id))
            {
                list.Add(item);
            }
        }
        return list.ToArray();
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<QuestEntity>(obj);
}

