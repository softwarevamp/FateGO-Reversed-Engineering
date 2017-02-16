using System;

public class ShopEventListViewItem : ListViewItem
{
    protected long closedAt;
    protected EventEntity eventEntity;
    protected int eventId;
    protected bool isFinished;
    protected long openedAt;

    public ShopEventListViewItem(int index, int eventId) : base(index)
    {
        this.eventId = eventId;
        this.eventEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.EVENT).getEntityFromId<EventEntity>(eventId);
        SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ShopMaster>(DataNameKind.Kind.SHOP).GetEnableEventPeriod(out this.openedAt, out this.closedAt, eventId);
        if (this.closedAt > 0L)
        {
            this.isFinished = NetworkManager.getTime() > this.closedAt;
        }
        else
        {
            this.isFinished = false;
        }
    }

    ~ShopEventListViewItem()
    {
    }

    private string ToString() => 
        ("Item " + this.NameText);

    public long ChangeEndedAt =>
        this.closedAt;

    public long ChangeStartedAt =>
        this.openedAt;

    public string DetailText
    {
        get
        {
            if (this.eventEntity != null)
            {
                return this.eventEntity.detail;
            }
            return LocalizationManager.GetUnknownName();
        }
    }

    public EventEntity Event =>
        this.eventEntity;

    public long EventEndedAt =>
        ((this.eventEntity == null) ? 0L : this.eventEntity.endedAt);

    public int EventId =>
        this.eventId;

    public long EventStartedAt =>
        ((this.eventEntity == null) ? 0L : this.eventEntity.startedAt);

    public bool IsFinished =>
        this.isFinished;

    public string NameText
    {
        get
        {
            if (this.eventEntity != null)
            {
                return this.eventEntity.name;
            }
            return "error";
        }
    }
}

