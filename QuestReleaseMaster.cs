using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

public class QuestReleaseMaster : DataMasterBase
{
    [CompilerGenerated]
    private static Comparison<QuestEntity> <>f__am$cache0;

    public QuestReleaseMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.QUEST_RELEASE);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new QuestReleaseEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<QuestReleaseEntity>(obj);

    public QuestReleaseEntity[] getListByQuestID(int qid)
    {
        List<QuestReleaseEntity> list = new List<QuestReleaseEntity>();
        for (int i = 0; i < base.list.Count; i++)
        {
            QuestReleaseEntity item = base.list[i] as QuestReleaseEntity;
            if ((item != null) && (item.getQuestId() == qid))
            {
                list.Add(item);
            }
        }
        return list.ToArray();
    }

    public QuestReleaseEntity[] getListByType(CondType.Kind type)
    {
        List<QuestReleaseEntity> list = new List<QuestReleaseEntity>();
        for (int i = 0; i < base.list.Count; i++)
        {
            QuestReleaseEntity item = base.list[i] as QuestReleaseEntity;
            if ((item != null) && (item.getType() == type))
            {
                list.Add(item);
            }
        }
        return list.ToArray();
    }

    public int[] GetQuestIdList(CondType.Kind type, int targetId, int value = -1)
    {
        QuestMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<QuestMaster>(DataNameKind.Kind.QUEST);
        List<QuestEntity> list = new List<QuestEntity>();
        for (int i = 0; i < base.list.Count; i++)
        {
            QuestReleaseEntity entity = base.list[i] as QuestReleaseEntity;
            if (((entity != null) && (entity.getType() == type)) && ((entity.getTargetId() == targetId) && ((value < 0) || (entity.getValue() == value))))
            {
                QuestEntity item = master.getEntityFromId<QuestEntity>(entity.getQuestId());
                if (item != null)
                {
                    list.Add(item);
                }
            }
        }
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = (a, b) => b.getPriority() - a.getPriority();
        }
        list.Sort(<>f__am$cache0);
        List<int> list2 = new List<int>();
        foreach (QuestEntity entity3 in list)
        {
            list2.Add(entity3.getQuestId());
        }
        return list2.ToArray();
    }
}

