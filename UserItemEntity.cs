using System;

public class UserItemEntity : DataEntityBase
{
    public int itemId;
    public int num;
    public long userId;

    public UserItemEntity()
    {
    }

    public UserItemEntity(long userId, int itemId)
    {
        this.userId = userId;
        this.itemId = itemId;
        this.num = 0;
    }

    public ItemEntity getItemInfo() => 
        SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ItemMaster>(DataNameKind.Kind.ITEM).GetItemData(this.itemId);

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.userId, ":", this.itemId };
        return string.Concat(objArray1);
    }

    public long getUserId() => 
        this.userId;

    public int getUserItemId() => 
        this.itemId;

    public int getUserItemNum() => 
        this.num;
}

