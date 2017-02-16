using System;

public class BoxGachaEntity : DataEntityBase
{
    public int[] baseIds;
    public string detailUrl;
    public int eventId;
    public int guideImageId;
    public int guideLimitCount;
    public int id;
    public int payTargetId;
    public int payType;
    public int payValue;
    public int priority;
    public int[] talkIds;

    public PayType.Type GetPayType() => 
        ((PayType.Type) this.payType);

    public override string getPrimarykey() => 
        (string.Empty + this.id);
}

