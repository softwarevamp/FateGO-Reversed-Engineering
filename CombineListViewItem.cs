using System;

public class CombineListViewItem : ListViewItem
{
    protected CombineListItemInfo info;

    public CombineListViewItem(int index, CombineListItemInfo info)
    {
        base.index = index;
        this.info = info;
    }

    ~CombineListViewItem()
    {
    }

    public string EventData =>
        this.info.eventData;

    public CombineListItemInfo Info =>
        this.info;

    public string SpriteName =>
        this.info.spriteName;

    public int Type =>
        this.info.type;
}

