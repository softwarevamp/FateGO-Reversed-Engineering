using System;

public class DropInfo
{
    public int limitCount;
    public int lv;
    public int num;
    public int objectId;
    public int rarity;
    public int type;

    public bool isItem() => 
        Gift.IsItem(this.type);

    public bool isServant() => 
        Gift.IsServant(this.type);
}

