using System;

public class EventPointItemListViewItem : ListViewItem
{
    protected EventRewardEntity eventRewardEnt;
    protected GiftEntity giftEnt;
    protected int iconId;
    protected bool isGetReward;
    protected ItemEntity itemEnt;
    protected string nameTxt;
    protected int needPoint;
    protected string numTxt;
    protected string rewardDetailTxt;
    protected EventRewardExtraEntity rewardExtraEnt;
    protected int rewardObjectId;
    protected ServantEntity svtEnt;

    public EventPointItemListViewItem(EventRewardEntity rewardData, bool isGet)
    {
        this.isGetReward = isGet;
        this.eventRewardEnt = rewardData;
        this.giftEnt = rewardData.getGiftData();
        this.rewardObjectId = 0;
        this.nameTxt = "Error";
        this.needPoint = 0;
        this.iconId = 0;
        this.rewardDetailTxt = string.Empty;
        switch (rewardData.type)
        {
            case 1:
                this.giftEnt = rewardData.getGiftData();
                this.setGiftData();
                break;

            case 2:
                this.rewardExtraEnt = rewardData.getSetRewardData();
                if (this.rewardExtraEnt != null)
                {
                    this.nameTxt = this.rewardExtraEnt.name;
                    this.iconId = this.rewardExtraEnt.iconId;
                    this.rewardDetailTxt = this.rewardExtraEnt.detail;
                }
                break;

            case 3:
            {
                EventRewardSetEntity entity = rewardData.getRewardSetData();
                if (entity != null)
                {
                    this.nameTxt = entity.name;
                    this.iconId = entity.iconId;
                    this.rewardDetailTxt = entity.detail;
                }
                break;
            }
        }
        this.needPoint = rewardData.point;
    }

    ~EventPointItemListViewItem()
    {
    }

    private void setGiftData()
    {
        if (this.giftEnt != null)
        {
            this.giftEnt.GetInfo(out this.nameTxt, out this.numTxt);
            switch (this.giftEnt.type)
            {
                case 1:
                case 6:
                case 7:
                    this.svtEnt = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.giftEnt.objectId);
                    break;

                case 2:
                    this.itemEnt = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.ITEM).getEntityFromId<ItemEntity>(this.giftEnt.objectId);
                    break;
            }
            this.rewardObjectId = this.giftEnt.objectId;
        }
    }

    public RewardType.Type eventRewardType =>
        ((RewardType.Type) this.eventRewardEnt.type);

    public int GetEventId =>
        this.eventRewardEnt.eventId;

    public bool IsGetReward =>
        this.isGetReward;

    public ItemEntity ItemEntity
    {
        get
        {
            if (this.itemEnt != null)
            {
                return this.itemEnt;
            }
            return null;
        }
    }

    public string NameText =>
        this.nameTxt;

    public string needPointTxt =>
        string.Format(LocalizationManager.Get("EVENT_POINT_ITEM_REQUIRED_POINT"), this.needPoint);

    public string NumText =>
        this.numTxt;

    public string RewardDetailTXt =>
        this.rewardDetailTxt;

    public int RewardObjId =>
        this.rewardObjectId;

    public int SetExtraIconId =>
        this.iconId;

    public ServantEntity SvtEntity
    {
        get
        {
            if (this.svtEnt != null)
            {
                return this.svtEnt;
            }
            return null;
        }
    }

    public Gift.Type Type
    {
        get
        {
            if (this.giftEnt != null)
            {
                return (Gift.Type) this.giftEnt.type;
            }
            return (Gift.Type) 0;
        }
    }
}

