using System;

public class QuestClearHeroineInfo
{
    public bool isChangeLimitcnt;
    public bool isChangeTreasureDvc;
    public int oldFriendShipRank;
    public UserServantEntity oldUsrSvtData;
    public int treasureDvcId;
    public int treasureDvcLv;

    public bool IsUpFriendShipRank() => 
        (this.oldFriendShipRank >= 0);
}

