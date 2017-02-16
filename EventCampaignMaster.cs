using System;

public class EventCampaignMaster : DataMasterBase
{
    public EventCampaignMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.EVENT_CAMPAIGN);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new EventCampaignEntity[1]);
        }
    }

    public EventCampaignEntity getData(int event_id)
    {
        foreach (DataEntityBase base2 in base.list)
        {
            EventCampaignEntity entity = base2 as EventCampaignEntity;
            if ((entity != null) && (entity.getEventId() == event_id))
            {
                return entity;
            }
        }
        return null;
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<EventCampaignEntity>(obj);

    public EventCampaignEntity GetTargetEntitiyList(CombineAdjustTarget.TYPE targetType)
    {
        int count = base.list.Count;
        int num2 = (int) targetType;
        for (int i = 0; i < count; i++)
        {
            EventCampaignEntity entity = base.list[i] as EventCampaignEntity;
            if ((entity != null) && ((num2 <= 0) || (num2 == entity.target)))
            {
                return entity;
            }
        }
        return null;
    }
}

