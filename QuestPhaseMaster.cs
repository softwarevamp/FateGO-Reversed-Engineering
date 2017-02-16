using System;
using System.Collections.Generic;

public class QuestPhaseMaster : DataMasterBase
{
    public QuestPhaseMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.QUEST_PHASE);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new QuestPhaseEntity[1]);
        }
    }

    public QuestPhaseEntity[] getList(int iQuestID)
    {
        List<QuestPhaseEntity> list = new List<QuestPhaseEntity>();
        for (int i = 0; i < base.list.Count; i++)
        {
            QuestPhaseEntity item = base.list[i] as QuestPhaseEntity;
            if ((item != null) && (item.getQuestId() == iQuestID))
            {
                list.Add(item);
            }
        }
        return list.ToArray();
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<QuestPhaseEntity>(obj);
}

