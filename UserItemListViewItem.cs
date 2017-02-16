using System;

public class UserItemListViewItem : ListViewItem
{
    private int dispPriority;
    private UserItemData itemData;
    private int itemId;
    private int itemImgId;
    private string itemName;
    private int itemNum;
    private int itemType;

    public UserItemListViewItem(UserItemData usrItemData)
    {
        this.itemData = usrItemData;
        this.itemId = usrItemData.itemId;
        this.itemType = usrItemData.type;
        this.dispPriority = usrItemData.dispPriority;
        this.itemImgId = usrItemData.itemImgId;
        this.itemName = usrItemData.name;
        this.itemNum = usrItemData.num;
    }

    ~UserItemListViewItem()
    {
    }

    public int DispPriority =>
        this.dispPriority;

    public int ItemId =>
        this.itemId;

    public int ItemImgId =>
        this.itemImgId;

    public string ItemName =>
        this.itemName;

    public int ItemNum =>
        this.itemNum;

    public int ItemType =>
        this.itemType;

    public UserItemData userItemData =>
        this.itemData;
}

