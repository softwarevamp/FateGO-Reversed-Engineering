using System;

public class PresentBoxNotificationListViewItem : ListViewItem
{
    protected UserPresentBoxEntity entity;
    protected ItemEntity itemEntity;
    protected string presentCount;
    protected string presentName;
    protected ServantEntity svtEntity;

    public PresentBoxNotificationListViewItem(int index, UserPresentBoxEntity e) : base(index)
    {
        this.entity = e;
        this.itemEntity = null;
        this.svtEntity = null;
        e.GetInfo(out this.presentName, out this.presentCount);
        switch (e.giftType)
        {
            case 1:
            case 6:
            case 7:
                this.svtEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(e.objectId);
                break;

            case 2:
                this.itemEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.ITEM).getEntityFromId<ItemEntity>(e.objectId);
                break;
        }
    }

    ~PresentBoxNotificationListViewItem()
    {
    }

    private string ToString() => 
        ("Present " + this.NameText);

    public string CountText =>
        this.presentCount;

    public Gift.Type GiftType
    {
        get
        {
            if (this.entity != null)
            {
                return (Gift.Type) this.entity.giftType;
            }
            return (Gift.Type) 0;
        }
    }

    public ItemEntity Item =>
        this.itemEntity;

    public string NameText =>
        this.presentName;

    public int ObjactId
    {
        get
        {
            if (this.entity != null)
            {
                return this.entity.objectId;
            }
            return 0;
        }
    }

    public ServantEntity Servant =>
        this.svtEntity;

    public UserPresentBoxEntity UserPresentBox =>
        this.entity;
}

