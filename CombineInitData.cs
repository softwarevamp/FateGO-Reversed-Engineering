using System;
using System.Collections.Generic;
using UnityEngine;

public class CombineInitData : MonoBehaviour
{
    private EventEntity[] combineEventList;
    private List<int> eventIdList = new List<int>();

    public EventCampaignEntity getCombineEventData(int eventId)
    {
        EventCampaignEntity[] entityArray = SingletonMonoBehaviour<DataManager>.Instance.getEntitys<EventCampaignEntity>(DataNameKind.Kind.EVENT_CAMPAIGN);
        Debug.Log("***!!! Event Combine Data : " + entityArray.Length);
        if (entityArray != null)
        {
            for (int i = 0; i < entityArray.Length; i++)
            {
                EventCampaignEntity entity = entityArray[i];
                if (entity.eventId == eventId)
                {
                    this.eventIdList.Add(entity.eventId);
                    return entity;
                }
            }
        }
        return null;
    }

    public EventEntity[] getCombineEventList() => 
        this.combineEventList;

    public EventCampaignEntity getCombineExpEventData(int eventId) => 
        SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventCampaignMaster>(DataNameKind.Kind.EVENT_CAMPAIGN).GetTargetEntitiyList(CombineAdjustTarget.TYPE.COMBINE_EXP);

    public EventCampaignEntity getCombineQpEventData(int eventId) => 
        SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventCampaignMaster>(DataNameKind.Kind.EVENT_CAMPAIGN).GetTargetEntitiyList(CombineAdjustTarget.TYPE.COMBINE_QP);

    public void getEventData()
    {
        EventMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventMaster>(DataNameKind.Kind.EVENT);
        EventEntity[] enableEntitiyList = master.GetEnableEntitiyList(GameEventType.TYPE.COMBINE_CAMPAIGN, true);
        EventEntity[] collection = master.GetEnableEntitiyList(GameEventType.TYPE.SVTEQUIP_COMBINE_CAMPAIGN, true);
        List<EventEntity> list = new List<EventEntity>(enableEntitiyList.Length + collection.Length);
        list.AddRange(enableEntitiyList);
        list.AddRange(collection);
        this.combineEventList = list.ToArray();
    }

    public List<int> getEventIdList() => 
        this.eventIdList;
}

