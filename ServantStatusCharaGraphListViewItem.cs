using System;

public class ServantStatusCharaGraphListViewItem : ListViewItem
{
    protected int imageLimitCount;
    protected ServantStatusListViewItem mainInfo;

    public ServantStatusCharaGraphListViewItem(int index, ServantStatusListViewItem mainInfo, int imageLimitCount) : base(index)
    {
        this.mainInfo = mainInfo;
        this.imageLimitCount = imageLimitCount;
    }

    ~ServantStatusCharaGraphListViewItem()
    {
    }

    public int ImageLimitCount =>
        this.imageLimitCount;

    public ServantStatusListViewItem MainInfo =>
        this.mainInfo;
}

