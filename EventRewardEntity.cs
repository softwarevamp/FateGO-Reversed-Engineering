using System;
using System.Runtime.InteropServices;

public class EventRewardEntity : DataEntityBase
{
    public int eventId;
    public int giftId;
    public int point;
    public int presentMessageId;
    public int type;

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

    public void GetInfo(out string nameText, out string countText)
    {
        nameText = string.Empty;
        countText = string.Empty;
        if (this.type == 1)
        {
            string str;
            string str2;
            this.getGiftData().GetInfo(out str, out str2);
            nameText = str;
            countText = str2;
        }
        else
        {
            EventRewardExtraEntity entity2 = this.getSetRewardData();
            if (entity2 != null)
            {
                nameText = entity2.name;
            }
        }
    }

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.eventId, ":", this.point };
        return string.Concat(objArray1);
    }

    public EventRewardSetEntity getRewardSetData() => 
        SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.EVENT_REWARD_SET).getEntityFromId<EventRewardSetEntity>(2, this.eventId, this.giftId);

    public EventRewardExtraEntity getSetRewardData()
    {
        foreach (EventRewardExtraEntity entity in SingletonMonoBehaviour<DataManager>.Instance.getEntitys<EventRewardExtraEntity>(DataNameKind.Kind.EVENT_REWARD_EXTRA))
        {
            if ((entity.eventId == this.eventId) && (entity.point == this.point))
            {
                return entity;
            }
        }
        return null;
    }

    public bool isQp()
    {
        if (this.type == 1)
        {
            GiftEntity entity = this.getGiftData();
            if (entity != null)
            {
                return entity.isQp();
            }
        }
        return false;
    }
}

