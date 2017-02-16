using System;

public class EventMissionEntity : DataEntityBase
{
    public long closedAt;
    public string detail;
    public int dispNo;
    public long endedAt;
    public int eventId;
    public int giftId;
    public int id;
    public string name;
    public int rewardType;
    public long startedAt;

    public GiftEntity getGiftData()
    {
        foreach (GiftEntity entity in SingletonMonoBehaviour<DataManager>.Instance.getEntitys<GiftEntity>(DataNameKind.Kind.GIFT))
        {
            if (entity.id == this.giftId)
            {
                return entity;
            }
        }
        return null;
    }

    public override string getPrimarykey() => 
        (string.Empty + this.id);

    public EventRewardSetEntity getSetRewardData() => 
        SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.EVENT_REWARD_SET).getEntityFromId<EventRewardSetEntity>(3, this.eventId, this.id);

    public bool isNowMission()
    {
        long num = NetworkManager.getTime();
        return ((num >= this.startedAt) && (num <= this.endedAt));
    }
}

