using System;

public class UserPresentListViewItem : ListViewItem
{
    public bool blocked;
    public bool checkBoxed;
    public int checkCount;
    protected EquipEntity equipEnt;
    protected int giftType;
    protected ItemEntity itemEnt;
    protected long presentId;
    protected int presentImgId;
    protected string presentMsg;
    protected string presentName;
    protected string presentNum;
    protected ServantEntity svtEnt;
    protected UserPresentBoxEntity usrPresentEnt;

    public UserPresentListViewItem(int index, UserPresentBoxEntity presentData) : base(index)
    {
        this.svtEnt = null;
        this.itemEnt = null;
        this.checkBoxed = false;
        this.checkCount = -1;
        this.blocked = false;
        this.usrPresentEnt = presentData;
        presentData.GetInfo(out this.presentName, out this.presentNum);
        this.presentId = presentData.presentId;
        this.giftType = presentData.giftType;
        this.presentMsg = presentData.message;
        switch (this.giftType)
        {
            case 1:
            case 6:
            case 7:
                this.svtEnt = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(presentData.objectId);
                break;

            case 2:
                this.itemEnt = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.ITEM).getEntityFromId<ItemEntity>(presentData.objectId);
                break;

            case 5:
                this.equipEnt = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.EQUIP).getEntityFromId<EquipEntity>(presentData.objectId);
                break;
        }
    }

    ~UserPresentListViewItem()
    {
    }

    public void setBlocked(bool blocked)
    {
        this.blocked = blocked;
        if (base.viewObject != null)
        {
            ((UserPresentListViewObject) base.viewObject).setBlocked(blocked);
        }
    }

    public void setCheckBoxed(bool checkBoxed, int count)
    {
        this.checkBoxed = checkBoxed;
        this.checkCount = count;
        if (base.viewObject != null)
        {
            ((UserPresentListViewObject) base.viewObject).setCheckBoxed(checkBoxed, count);
        }
    }

    public override bool SetSortValue(ListViewSort sort)
    {
        base.sortValue1 = this.usrPresentEnt.presentId;
        return true;
    }

    public int ImageId =>
        this.presentImgId;

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

    public int ListType =>
        this.giftType;

    public string MsgText =>
        this.presentMsg;

    public string NameText
    {
        get
        {
            if ((this.svtEnt != null) && Gift.IsServant(this.giftType))
            {
                return this.svtEnt.name;
            }
            if ((this.itemEnt != null) && (this.giftType == 2))
            {
                return this.itemEnt.name;
            }
            if ((this.equipEnt != null) && (this.giftType == 5))
            {
                return this.equipEnt.name;
            }
            return "error";
        }
    }

    public string NumText =>
        this.presentNum;

    public long PresentId =>
        this.presentId;

    public int PresentObjId
    {
        get
        {
            if (this.usrPresentEnt != null)
            {
                return this.usrPresentEnt.objectId;
            }
            return 0;
        }
    }

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
            if (this.usrPresentEnt != null)
            {
                return (Gift.Type) this.usrPresentEnt.giftType;
            }
            return (Gift.Type) 0;
        }
    }

    public UserPresentBoxEntity UserPresentEntity
    {
        get
        {
            if (this.usrPresentEnt != null)
            {
                return this.usrPresentEnt;
            }
            return null;
        }
    }
}

