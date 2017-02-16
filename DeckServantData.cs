using System;

public class DeckServantData
{
    public int id;
    public bool isFollowerSvt;
    public long userId;
    public long[] userSvtEquipIds = new long[BalanceConfig.SvtEquipMax];
    public long userSvtId;
}

