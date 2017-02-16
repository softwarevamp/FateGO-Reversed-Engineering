using System;

public class BoxGachaItemListViewItem : ListViewItem
{
    protected int currenNum;
    protected int currentEventId;
    protected string detailTxt;
    protected string extraDetailTxt;
    protected BoxGachaExtraEntity extraEnt;
    protected BoxGachaBaseEntity gachaBaseData;
    protected GiftEntity giftEnt;
    protected int giftType;
    protected bool isDraw;
    protected bool isRare;
    protected ItemEntity itemEnt;
    protected int maxNum;
    protected string nameTxt;
    protected ServantEntity svtEnt;
    protected int targetImgId;
    protected int targetObjectId;

    public BoxGachaItemListViewItem(BoxGachaBaseEntity data, int eventId, int boxGachaId, bool isDraw)
    {
        this.gachaBaseData = data;
        this.currentEventId = eventId;
        this.isRare = data.isRare;
        this.isDraw = isDraw;
        BoxGachaHistoryEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<BoxGachaHistoryMaster>(DataNameKind.Kind.BOX_GACHA_HISTORY).getEntityFromId<BoxGachaHistoryEntity>(boxGachaId);
        int num = 0;
        if (entity != null)
        {
            num = entity.getDrawNum(data.no);
        }
        this.currenNum = data.maxNum - num;
        this.maxNum = data.maxNum;
        this.detailTxt = data.detail;
        switch (data.type)
        {
            case 1:
                this.giftEnt = data.getGiftData();
                this.giftType = this.giftEnt.type;
                this.setGiftData();
                break;

            case 2:
            case 3:
            {
                EventRewardSetEntity entity2 = data.getRewardSetData(this.currentEventId);
                this.nameTxt = entity2.name;
                this.targetImgId = entity2.iconId;
                this.extraDetailTxt = entity2.detail;
                break;
            }
        }
    }

    ~BoxGachaItemListViewItem()
    {
    }

    private void setGiftData()
    {
        if (this.giftEnt != null)
        {
            string str;
            this.giftEnt.GetInfo(out this.nameTxt, out str);
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
            this.targetObjectId = this.giftEnt.objectId;
        }
    }

    public int CurrentNum =>
        this.currenNum;

    public string DetailTxt =>
        this.detailTxt;

    public string ExtraDetailTXt =>
        this.extraDetailTxt;

    public RewardType.Type GachaBaseType =>
        ((RewardType.Type) this.gachaBaseData.type);

    public BoxGachaBaseEntity GetBaseData =>
        this.gachaBaseData;

    public int GetEventId =>
        this.currentEventId;

    public int GetIconId =>
        this.gachaBaseData.iconId;

    public Gift.Type GiftType
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

    public bool IsDraw =>
        this.isDraw;

    public bool IsRare =>
        this.isRare;

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

    public int MaxNum =>
        this.maxNum;

    public string NameTxt =>
        this.nameTxt;

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

    public int TargetImgId =>
        this.targetImgId;

    public int TargetObjectId =>
        this.targetObjectId;
}

