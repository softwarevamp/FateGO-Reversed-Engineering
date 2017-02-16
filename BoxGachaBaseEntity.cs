using System;

public class BoxGachaBaseEntity : DataEntityBase
{
    public string detail;
    public int iconId;
    public int id;
    public bool isRare;
    public int maxNum;
    public int no;
    public int priority;
    public int targetId;
    public int type;

    public GiftEntity getGiftData()
    {
        foreach (GiftEntity entity in SingletonMonoBehaviour<DataManager>.Instance.getEntitys<GiftEntity>(DataNameKind.Kind.GIFT))
        {
            if (entity.id == this.targetId)
            {
                return entity;
            }
        }
        return null;
    }

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.id, ":", this.no };
        return string.Concat(objArray1);
    }

    public EventRewardSetEntity getRewardSetData(int eventId) => 
        SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.EVENT_REWARD_SET).getEntityFromId<EventRewardSetEntity>(1, eventId, this.targetId);
}

