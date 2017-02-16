using System;
using System.Collections.Generic;

public class QuestGroupMaster : DataMasterBase
{
    public QuestGroupMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.QUEST_GROUP);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new QuestGroupEntity[1]);
        }
    }

    public int GetEventId(int questId) => 
        this.GetGroupId(questId, QuestGroupType.Type.EVENT_QUEST);

    public int GetGroupId(int questId, QuestGroupType.Type type)
    {
        int count = base.list.Count;
        for (int i = 0; i < count; i++)
        {
            QuestGroupEntity entity = base.list[i] as QuestGroupEntity;
            if (((entity != null) && (entity.questId == questId)) && (entity.type == type))
            {
                return entity.groupId;
            }
        }
        return 0;
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<QuestGroupEntity>(obj);

    public int[] GetQuestIdListByEventId(int eventId) => 
        this.GetQuestIdListByGroupId(eventId, QuestGroupType.Type.EVENT_QUEST);

    public int[] GetQuestIdListByGroupId(int groupId, QuestGroupType.Type type)
    {
        List<int> list = new List<int>();
        int count = base.list.Count;
        for (int i = 0; i < count; i++)
        {
            QuestGroupEntity entity = base.list[i] as QuestGroupEntity;
            if (((entity != null) && (entity.type == type)) && (entity.groupId == groupId))
            {
                list.Add(entity.questId);
            }
        }
        return list.ToArray();
    }
}

