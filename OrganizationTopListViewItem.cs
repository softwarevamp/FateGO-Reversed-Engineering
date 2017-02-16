using System;

public class OrganizationTopListViewItem : ListViewItem
{
    protected OrganizationTopItemInfo info;

    public OrganizationTopListViewItem(int index, OrganizationTopItemInfo info) : base(index)
    {
        this.info = info;
    }

    ~OrganizationTopListViewItem()
    {
    }

    private string ToString() => 
        ("OrganizationTopItem " + this.info.textCode);

    public string EventData =>
        this.info.eventData;

    public string ImageName =>
        this.info.imageName;

    public OrganizationTopItemInfo Info =>
        this.info;

    public string InfoText =>
        LocalizationManager.Get(this.info.textCode);
}

