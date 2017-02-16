using System;

public class BattleDropItem
{
    public bool isNew;
    public int limitCount;
    public int num;
    public int objectId;
    public int type;
    public long userSvtId;

    public void setData(DropInfo info)
    {
        this.type = info.type;
        this.objectId = info.objectId;
        this.limitCount = info.limitCount;
        this.num = info.num;
    }
}

