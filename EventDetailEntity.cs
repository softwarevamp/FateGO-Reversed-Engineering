using System;

public class EventDetailEntity : DataEntityBase
{
    public int afterBgmId;
    public int bgmId;
    public string condMessage;
    public int condQuestId;
    public int eventId;
    public int[] guideImageIds;
    public int[] guideLimitCounts;
    public bool isBonusSkill;
    public bool isBoxGacha;
    public bool isEventPoint;
    public bool isExchangeShop;
    public bool isMission;
    public bool isRanking;
    public int pointImageId;
    public int rewardPageBgId;
    public int terminalDisplayType;
    public string[] tutorialImageIds;

    public int getBgmId()
    {
        EventEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.EVENT).getEntityFromId<EventEntity>(this.eventId);
        int num = 0;
        if (entity != null)
        {
            num = (NetworkManager.getTime() > entity.endedAt) ? this.afterBgmId : this.bgmId;
        }
        return num;
    }

    public override string getPrimarykey() => 
        (string.Empty + this.eventId);

    public bool IsReward() => 
        ((this.isBoxGacha || this.isEventPoint) || this.isMission);
}

