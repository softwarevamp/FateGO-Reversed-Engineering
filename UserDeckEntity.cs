using System;

public class UserDeckEntity : DataEntityBase
{
    public int cost;
    public DeckServant deckInfo;
    public int deckNo;
    public long id;
    public string name = string.Empty;
    public long userId;

    public int getCost()
    {
        UserServantMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantMaster>(DataNameKind.Kind.USER_SERVANT);
        int num = 0;
        for (int i = 0; i < this.deckInfo.svts.Length; i++)
        {
            DeckServantData data = this.deckInfo.svts[i];
            long[] equipList = this.GetEquipList(i);
            if ((((data.id > 0) && (data.id <= BalanceConfig.DeckMemberMax)) && !data.isFollowerSvt) && (data.userSvtId > 0L))
            {
                UserServantEntity entity = master.getEntityFromId<UserServantEntity>(data.userSvtId);
                if ((entity != null) && (entity.svtId > 0))
                {
                    num += entity.getCost();
                    for (int j = 0; j < equipList.Length; j++)
                    {
                        if (equipList[j] > 0L)
                        {
                            UserServantEntity entity2 = master.getEntityFromId<UserServantEntity>(equipList[j]);
                            if ((entity2 != null) && (entity2.svtId > 0))
                            {
                                num += entity.getCost();
                            }
                        }
                    }
                }
            }
        }
        return num;
    }

    public long[] GetEquipList(int menber)
    {
        if (this.deckInfo != null)
        {
            return this.deckInfo.GetEquipList(menber);
        }
        return new long[BalanceConfig.SvtEquipMax];
    }

    public long[] GetEquipList(long userSvtId)
    {
        if (this.deckInfo != null)
        {
            return this.deckInfo.GetEquipList(userSvtId);
        }
        return new long[BalanceConfig.SvtEquipMax];
    }

    public int GetFollowerIndex()
    {
        for (int i = 0; i < this.deckInfo.svts.Length; i++)
        {
            DeckServantData data = this.deckInfo.svts[i];
            if (((data.id > 0) && (data.id <= BalanceConfig.DeckMemberMax)) && data.isFollowerSvt)
            {
                return data.id;
            }
        }
        return 0;
    }

    public override string getPrimarykey() => 
        (string.Empty + this.id);

    public UserServantEntity GetUserServant(int menber)
    {
        if (this.deckInfo != null)
        {
            return this.deckInfo.GetUserServant(menber);
        }
        return null;
    }

    public UserServantEntity[] GetUserServantList()
    {
        UserServantMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantMaster>(DataNameKind.Kind.USER_SERVANT);
        UserServantEntity[] entityArray = new UserServantEntity[BalanceConfig.DeckMemberMax];
        for (int i = 0; i < this.deckInfo.svts.Length; i++)
        {
            DeckServantData data = this.deckInfo.svts[i];
            if ((((data.id > 0) && (data.id <= BalanceConfig.DeckMemberMax)) && !data.isFollowerSvt) && (data.userSvtId > 0L))
            {
                entityArray[data.id - 1] = master.getEntityFromId<UserServantEntity>(data.userSvtId);
            }
        }
        return entityArray;
    }

    public bool IsEquip(long userSvtId) => 
        ((this.deckInfo != null) && this.deckInfo.IsEquip(userSvtId));

    public bool IsMember(long userSvtId)
    {
        for (int i = 0; i < this.deckInfo.svts.Length; i++)
        {
            DeckServantData data = this.deckInfo.svts[i];
            if (((data.id > 0) && (data.id <= BalanceConfig.DeckMemberMax)) && (!data.isFollowerSvt && (data.userSvtId == userSvtId)))
            {
                return true;
            }
        }
        return false;
    }
}

