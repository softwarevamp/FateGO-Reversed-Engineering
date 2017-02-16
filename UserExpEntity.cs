using System;

public class UserExpEntity : DataEntityBase
{
    public int addActMax;
    public int addCostMax;
    public int addFriendMax;
    public int addMagicMax;
    public string comment;
    public int exp;
    public int giftId;
    public int lv;

    public override string getPrimarykey() => 
        (string.Empty + this.lv);
}

