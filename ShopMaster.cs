using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

public class ShopMaster : DataMasterBase
{
    [CompilerGenerated]
    private static Comparison<ShopEntity> <>f__am$cache0;
    [CompilerGenerated]
    private static Comparison<ShopEntity> <>f__am$cache1;
    [CompilerGenerated]
    private static Comparison<ShopEntity> <>f__am$cache2;
    [CompilerGenerated]
    private static Comparison<ShopEntity> <>f__am$cache3;
    [CompilerGenerated]
    private static Comparison<EventEntity> <>f__am$cache4;
    [CompilerGenerated]
    private static Comparison<EventEntity> <>f__am$cache5;
    [CompilerGenerated]
    private static Comparison<EventEntity> <>f__am$cache6;
    [CompilerGenerated]
    private static Comparison<EventEntity> <>f__am$cache7;
    [CompilerGenerated]
    private static Comparison<ItemEntity> <>f__am$cache8;

    public ShopMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.SHOP);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new ShopEntity[1]);
        }
    }

    public ShopEntity[] GetEnableEntitiyList(Purchase.Type purchaseType, PayType.Type payType)
    {
        long nowTime = NetworkManager.getTime();
        int count = base.list.Count;
        int num3 = (int) purchaseType;
        int num4 = (int) payType;
        List<ShopEntity> list = new List<ShopEntity>();
        for (int i = 0; i < count; i++)
        {
            ShopEntity item = base.list[i] as ShopEntity;
            if ((((item != null) && (item.eventId == 0)) && ((num3 <= 0) || (num3 == item.purchaseType))) && ((num4 == item.payType) && item.IsEnable(nowTime)))
            {
                list.Add(item);
            }
        }
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = (a, b) => a.priority - b.priority;
        }
        list.Sort(<>f__am$cache0);
        return list.ToArray();
    }

    public ShopEntity[] GetEnableEventEntitiyList(int eventId)
    {
        long nowTime = NetworkManager.getTime();
        List<ShopEntity> list = new List<ShopEntity>();
        int count = base.list.Count;
        for (int i = 0; i < count; i++)
        {
            ShopEntity item = base.list[i] as ShopEntity;
            if (((item != null) && (item.eventId == eventId)) && item.IsEnable(nowTime))
            {
                list.Add(item);
            }
        }
        if (<>f__am$cache3 == null)
        {
            <>f__am$cache3 = (a, b) => a.priority - b.priority;
        }
        list.Sort(<>f__am$cache3);
        return list.ToArray();
    }

    public int[] GetEnableEventIdList()
    {
        long nowTime = NetworkManager.getTime();
        EventMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventMaster>(DataNameKind.Kind.EVENT);
        List<EventEntity> list = new List<EventEntity>();
        int count = base.list.Count;
        for (int i = 0; i < count; i++)
        {
            ShopEntity entity = base.list[i] as ShopEntity;
            if ((entity != null) && (entity.eventId != 0))
            {
                for (int k = 0; k < list.Count; k++)
                {
                    if (list[k].id == entity.eventId)
                    {
                        entity = null;
                        break;
                    }
                }
                if ((entity != null) && entity.IsEnable(nowTime))
                {
                    EventEntity item = master.getEntityFromId<EventEntity>(entity.eventId);
                    list.Add(item);
                }
            }
        }
        if (<>f__am$cache7 == null)
        {
            <>f__am$cache7 = (Comparison<EventEntity>) ((a, b) => ((a.finishedAt <= 0L) ? ((b.finishedAt <= 0L) ? ((Comparison<EventEntity>) 0) : ((Comparison<EventEntity>) 1)) : ((Comparison<EventEntity>) ((int) (a.finishedAt - b.finishedAt)))));
        }
        list.Sort(<>f__am$cache7);
        int[] numArray = new int[list.Count];
        for (int j = 0; j < list.Count; j++)
        {
            numArray[j] = list[j].id;
        }
        return numArray;
    }

    public bool GetEnableEventPeriod(out long openedAt, out long closedAt, int eventId)
    {
        long num = NetworkManager.getTime();
        openedAt = -1L;
        closedAt = -1L;
        int count = base.list.Count;
        for (int i = 0; i < count; i++)
        {
            ShopEntity entity = base.list[i] as ShopEntity;
            if (((entity != null) && (eventId == entity.eventId)) && (entity.openedAt < num))
            {
                if ((openedAt < 0L) || (openedAt > entity.openedAt))
                {
                    openedAt = entity.openedAt;
                }
                if ((closedAt != 0) && ((entity.closedAt == 0) || (closedAt < entity.closedAt)))
                {
                    closedAt = entity.closedAt;
                }
            }
        }
        return ((openedAt >= 0L) && (closedAt >= 0L));
    }

    public int GetEnableMainEventId()
    {
        long nowTime = NetworkManager.getTime();
        EventMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventMaster>(DataNameKind.Kind.EVENT);
        ShopEntity entity = null;
        long finishedAt = 0L;
        int count = base.list.Count;
        for (int i = 0; i < count; i++)
        {
            ShopEntity entity2 = base.list[i] as ShopEntity;
            if (((entity2 != null) && (entity2.eventId != 0)) && entity2.IsEnable(nowTime))
            {
                EventEntity entity3 = master.getEntityFromId<EventEntity>(entity2.eventId);
                if (entity == null)
                {
                    entity = entity2;
                    finishedAt = entity3.finishedAt;
                }
                else if ((entity3.finishedAt > 0L) && (finishedAt > entity3.finishedAt))
                {
                    entity = entity2;
                    finishedAt = entity3.finishedAt;
                }
            }
        }
        return ((entity == null) ? 0 : entity.eventId);
    }

    public ShopEntity getEntityFromId(int id)
    {
        string key = string.Empty + id;
        if (base.lookup.ContainsKey(key))
        {
            return (base.lookup[key] as ShopEntity);
        }
        return null;
    }

    public ShopEntity[] GetEventEntitiyList(int eventId)
    {
        List<ShopEntity> list = new List<ShopEntity>();
        int count = base.list.Count;
        for (int i = 0; i < count; i++)
        {
            ShopEntity item = base.list[i] as ShopEntity;
            if ((item != null) && (item.eventId == eventId))
            {
                list.Add(item);
            }
        }
        if (<>f__am$cache1 == null)
        {
            <>f__am$cache1 = (a, b) => a.priority - b.priority;
        }
        list.Sort(<>f__am$cache1);
        return list.ToArray();
    }

    public int[] GetEventItemList(int eventId)
    {
        if (eventId <= 0)
        {
            return null;
        }
        if (base.list.Count == 0)
        {
            return null;
        }
        ItemMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ItemMaster>(DataNameKind.Kind.ITEM);
        List<ItemEntity> list = new List<ItemEntity>();
        foreach (ShopEntity entity in base.list)
        {
            if (entity.eventId == eventId)
            {
                int length = entity.itemIds.Length;
                for (int j = 0; j < length; j++)
                {
                    if (entity.itemIds[j] <= 0)
                    {
                        continue;
                    }
                    bool flag = true;
                    for (int k = 0; k < list.Count; k++)
                    {
                        if (list[k].id == entity.itemIds[j])
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (flag)
                    {
                        list.Add(master.getEntityFromId<ItemEntity>(entity.itemIds[j]));
                    }
                }
            }
        }
        if (<>f__am$cache8 == null)
        {
            <>f__am$cache8 = (a, b) => a.priority - b.priority;
        }
        list.Sort(<>f__am$cache8);
        int count = list.Count;
        int[] numArray = new int[count];
        for (int i = 0; i < count; i++)
        {
            numArray[i] = list[i].id;
        }
        return numArray;
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<ShopEntity>(obj);

    public ShopEntity[] GetOpenedEventEntitiyList(int eventId)
    {
        long nowTime = NetworkManager.getTime();
        List<ShopEntity> list = new List<ShopEntity>();
        int count = base.list.Count;
        for (int i = 0; i < count; i++)
        {
            ShopEntity item = base.list[i] as ShopEntity;
            if (((item != null) && (item.eventId == eventId)) && item.IsOpened(nowTime))
            {
                list.Add(item);
            }
        }
        if (<>f__am$cache2 == null)
        {
            <>f__am$cache2 = (a, b) => a.priority - b.priority;
        }
        list.Sort(<>f__am$cache2);
        return list.ToArray();
    }

    public int[] GetOpenedEventIdList()
    {
        long nowTime = NetworkManager.getTime();
        EventMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventMaster>(DataNameKind.Kind.EVENT);
        List<EventEntity> list = new List<EventEntity>();
        List<EventEntity> list2 = new List<EventEntity>();
        List<EventEntity> list3 = new List<EventEntity>();
        List<int> list4 = new List<int>();
        List<int> list5 = new List<int>();
        int count = base.list.Count;
        for (int i = 0; i < count; i++)
        {
            ShopEntity entity = base.list[i] as ShopEntity;
            if (list4.IndexOf(entity.baseShopId) >= 0)
            {
                if (list5.IndexOf(entity.baseShopId) < 0)
                {
                    list5.Add(entity.baseShopId);
                }
            }
            else
            {
                list4.Add(entity.baseShopId);
            }
        }
        for (int j = 0; j < count; j++)
        {
            ShopEntity entity2 = base.list[j] as ShopEntity;
            if (((entity2 != null) && (entity2.eventId != 0)) && entity2.IsOpened(nowTime))
            {
                EventEntity item = master.getEntityFromId<EventEntity>(entity2.eventId);
                if (item != null)
                {
                    if (item.IsOpen(false))
                    {
                        if ((list5.IndexOf(entity2.baseShopId) < 0) || this.IsOpenNoQuestEventShop(entity2.baseShopId))
                        {
                            if (list.IndexOf(item) < 0)
                            {
                                list.Add(item);
                            }
                        }
                        else
                        {
                            if (list2.IndexOf(item) < 0)
                            {
                                list2.Add(item);
                            }
                            entity2.closedAt = entity2.openedAt;
                        }
                    }
                    else if (entity2.IsClosed(nowTime))
                    {
                        if (list2.IndexOf(item) < 0)
                        {
                            list2.Add(item);
                        }
                    }
                    else if (list3.IndexOf(item) < 0)
                    {
                        list3.Add(item);
                    }
                }
            }
        }
        for (int k = 0; k < list.Count; k++)
        {
            EventEntity entity4 = list[k];
            list3.Remove(entity4);
            list2.Remove(entity4);
        }
        if (<>f__am$cache4 == null)
        {
            <>f__am$cache4 = (a, b) => (a.endedAt == b.endedAt) ? ((Comparison<EventEntity>) (a.id - b.id)) : ((Comparison<EventEntity>) ((int) (a.endedAt - b.endedAt)));
        }
        list.Sort(<>f__am$cache4);
        for (int m = 0; m < list3.Count; m++)
        {
            long num7;
            long num8;
            EventEntity entity5 = list3[m];
            list2.Remove(entity5);
            this.GetEnableEventPeriod(out num7, out num8, entity5.id);
            entity5.setShopClosedAt(num8);
        }
        if (<>f__am$cache5 == null)
        {
            <>f__am$cache5 = (a, b) => (int) a.cmpShopClosedAt(b);
        }
        list3.Sort(<>f__am$cache5);
        if (<>f__am$cache6 == null)
        {
            <>f__am$cache6 = (a, b) => (a.startedAt == b.startedAt) ? ((Comparison<EventEntity>) (a.id - b.id)) : ((Comparison<EventEntity>) ((int) (b.startedAt - a.startedAt)));
        }
        list2.Sort(<>f__am$cache6);
        int num9 = (list2.Count <= 10) ? list2.Count : 10;
        int num10 = (list.Count + list3.Count) + num9;
        int[] numArray = new int[num10];
        int num11 = 0;
        for (int n = 0; n < list.Count; n++)
        {
            numArray[num11++] = list[n].id;
        }
        for (int num13 = 0; num13 < list3.Count; num13++)
        {
            numArray[num11++] = list3[num13].id;
        }
        for (int num14 = 0; num14 < num9; num14++)
        {
            numArray[num11++] = list2[num14].id;
        }
        return numArray;
    }

    public int GetOpenMainEventId()
    {
        long nowTime = NetworkManager.getTime();
        EventMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventMaster>(DataNameKind.Kind.EVENT);
        ShopEntity entity = null;
        long finishedAt = 0L;
        int count = base.list.Count;
        for (int i = 0; i < count; i++)
        {
            ShopEntity entity2 = base.list[i] as ShopEntity;
            if (((entity2 != null) && (entity2.eventId != 0)) && entity2.IsEnable(nowTime))
            {
                EventEntity entity3 = master.getEntityFromId<EventEntity>(entity2.eventId);
                if (entity3.IsOpen(false))
                {
                    if (entity == null)
                    {
                        entity = entity2;
                        finishedAt = entity3.finishedAt;
                    }
                    else if ((entity3.finishedAt > 0L) && (finishedAt > entity3.finishedAt))
                    {
                        entity = entity2;
                        finishedAt = entity3.finishedAt;
                    }
                }
            }
        }
        if (entity != null)
        {
            return entity.eventId;
        }
        return this.GetEnableMainEventId();
    }

    public bool IsOpenNoQuestEvent(ShopEntity shopEntity)
    {
        List<int> list = new List<int>();
        List<int> list2 = new List<int>();
        for (int i = 0; i < base.list.Count; i++)
        {
            ShopEntity entity = base.list[i] as ShopEntity;
            if (list.IndexOf(entity.baseShopId) >= 0)
            {
                if (list2.IndexOf(entity.baseShopId) < 0)
                {
                    list2.Add(entity.baseShopId);
                }
            }
            else
            {
                list.Add(entity.baseShopId);
            }
        }
        if (list2.IndexOf(shopEntity.baseShopId) < 0)
        {
            return false;
        }
        return true;
    }

    public bool IsOpenNoQuestEventShop(int id)
    {
        for (int i = 0; i < base.list.Count; i++)
        {
            ShopEntity entity2 = base.list[i] as ShopEntity;
            if ((entity2.baseShopId == id) && (SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserShopMaster>(DataNameKind.Kind.USER_SHOP).getEntityFromId(NetworkManager.UserId, id).num > 0))
            {
                return false;
            }
        }
        return true;
    }
}

