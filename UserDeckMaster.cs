using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class UserDeckMaster : DataMasterBase
{
    public UserDeckMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.USER_DECK);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new UserDeckEntity[1]);
        }
    }

    public int getCost(long deckId) => 
        base.getEntityFromId<UserDeckEntity>(deckId).getCost();

    public UserDeckEntity[] getDeckList(long userId)
    {
        int count = base.list.Count;
        List<UserDeckEntity> list = new List<UserDeckEntity>();
        for (int i = 0; i < count; i++)
        {
            UserDeckEntity item = base.list[i] as UserDeckEntity;
            if (((item != null) && (item.userId == userId)) && (item.deckNo <= BalanceConfig.DeckMax))
            {
                list.Add(item);
            }
        }
        UserDeckEntity[] entityArray = new UserDeckEntity[(list.Count >= BalanceConfig.DeckMax) ? BalanceConfig.DeckMax : list.Count];
        for (int j = 0; j < list.Count; j++)
        {
            UserDeckEntity entity2 = list[j];
            if (entity2.deckNo <= entityArray.Length)
            {
                entityArray[entity2.deckNo - 1] = entity2;
            }
        }
        return entityArray;
    }

    public UserDeckEntity getEntityFromId(long deckId) => 
        base.getEntityFromId<UserDeckEntity>(deckId);

    public long[] getEquipList(long userSvtId)
    {
        UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
        UserDeckEntity entity2 = base.getEntityFromId<UserDeckEntity>(entity.activeDeckId);
        if (entity2 != null)
        {
            return entity2.GetEquipList(userSvtId);
        }
        return new long[BalanceConfig.SvtEquipMax];
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<UserDeckEntity>(obj);

    public long[] getPartyList(long userId)
    {
        int count = base.list.Count;
        List<long> collectList = new List<long>();
        for (int i = 0; i < count; i++)
        {
            UserDeckEntity entity = base.list[i] as UserDeckEntity;
            if (((entity != null) && (entity.userId == userId)) && (entity.deckNo <= BalanceConfig.DeckMax))
            {
                entity.deckInfo.CollectUserSvtId(collectList);
            }
        }
        return collectList.ToArray();
    }

    public void getPartyList(out long[] svtIdList, out long[] equipIdList, long userId)
    {
        int count = base.list.Count;
        List<long> svtCollectList = new List<long>();
        List<long> equipCollectList = new List<long>();
        for (int i = 0; i < count; i++)
        {
            UserDeckEntity entity = base.list[i] as UserDeckEntity;
            if (((entity != null) && (entity.userId == userId)) && (entity.deckNo <= BalanceConfig.DeckMax))
            {
                entity.deckInfo.CollectUserSvtId(svtCollectList, equipCollectList);
            }
        }
        svtIdList = svtCollectList.ToArray();
        equipIdList = equipCollectList.ToArray();
    }

    public UserServantEntity[] GetUserServantListFromDeck()
    {
        UserServantEntity[] userServantList = null;
        UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
        userServantList = this.getEntityFromId(entity.activeDeckId).GetUserServantList();
        UserServantEntity[] entityArray2 = userServantList;
        for (int i = 0; i < entityArray2.Length; i++)
        {
            if (entityArray2[i] != null)
            {
                return userServantList;
            }
        }
        return this.getEntityFromId(entity.mainDeckId).GetUserServantList();
    }

    public bool IsEquip(long userSvtId)
    {
        int count = base.list.Count;
        for (int i = 0; i < count; i++)
        {
            UserDeckEntity entity = base.list[i] as UserDeckEntity;
            if (((entity != null) && (entity.deckNo <= BalanceConfig.DeckMax)) && entity.deckInfo.IsEquip(userSvtId))
            {
                return true;
            }
        }
        return false;
    }
}

