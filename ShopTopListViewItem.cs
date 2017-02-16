using System;

public class ShopTopListViewItem : ListViewItem
{
    protected ShopTopItemInfo info;
    protected bool isUse;

    public ShopTopListViewItem(int index, ShopTopItemInfo info, bool isUse) : base(index)
    {
        this.info = info;
        this.isUse = isUse;
    }

    ~ShopTopListViewItem()
    {
    }

    private string ToString() => 
        ("ShopTopItem " + this.info.textCode);

    public string EventData =>
        this.info.eventData;

    public string ImageName =>
        this.info.imageName;

    public ShopTopItemInfo Info =>
        this.info;

    public string InfoText =>
        LocalizationManager.Get(this.info.textCode);

    public bool IsPeriod =>
        ((this.info.kind == 5) && (SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ShopMaster>(DataNameKind.Kind.SHOP).GetEnableEventIdList().Length > 0));

    public bool IsUse =>
        this.isUse;
}

