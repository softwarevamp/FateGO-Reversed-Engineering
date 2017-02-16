using System;

public class EventCombineMaster : DataMasterBase
{
    public EventCombineMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.EVENT_COMBINE);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new EventCombineEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<EventCombineEntity>(obj);

    public EventCombineEntity GetTargetEntitiyList(CombineAdjustTarget.TYPE targetType)
    {
        int count = base.list.Count;
        int num2 = (int) targetType;
        for (int i = 0; i < count; i++)
        {
            EventCombineEntity entity = base.list[i] as EventCombineEntity;
            if ((entity != null) && ((num2 <= 0) || (num2 == entity.target)))
            {
                return entity;
            }
        }
        return null;
    }
}

