using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class BoxGachaMaster : DataMasterBase
{
    [CompilerGenerated]
    private static Comparison<BoxGachaEntity> <>f__am$cache0;
    [CompilerGenerated]
    private static Comparison<ItemEntity> <>f__am$cache1;

    public BoxGachaMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.BOX_GACHA);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new BoxGachaEntity[1]);
        }
    }

    public BoxGachaEntity[] getBoxGachaDataByEventId(int eventId)
    {
        List<BoxGachaEntity> list = new List<BoxGachaEntity>();
        int count = base.list.Count;
        for (int i = 0; i < count; i++)
        {
            BoxGachaEntity item = base.list[i] as BoxGachaEntity;
            if ((item != null) && ((eventId <= 0) || (eventId == item.eventId)))
            {
                list.Add(item);
            }
        }
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = (a, b) => b.priority - a.priority;
        }
        list.Sort(<>f__am$cache0);
        return list.ToArray();
    }

    public int[] GetEventItemList(int event_id)
    {
        List<int> list = new List<int>();
        foreach (BoxGachaEntity entity in this.getBoxGachaDataByEventId(event_id))
        {
            if (entity.GetPayType() == PayType.Type.EVENT_ITEM)
            {
                int payTargetId = entity.payTargetId;
                if (!list.Contains(payTargetId))
                {
                    list.Add(payTargetId);
                }
            }
        }
        ItemMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ItemMaster>(DataNameKind.Kind.ITEM);
        List<ItemEntity> list2 = new List<ItemEntity>();
        for (int i = 0; i < list.Count; i++)
        {
            ItemEntity item = master.getEntityFromId<ItemEntity>(list[i]);
            if (item != null)
            {
                list2.Add(item);
            }
        }
        if (<>f__am$cache1 == null)
        {
            <>f__am$cache1 = (a, b) => a.priority - b.priority;
        }
        list2.Sort(<>f__am$cache1);
        int[] numArray = new int[list2.Count];
        for (int j = 0; j < list2.Count; j++)
        {
            numArray[j] = list2[j].id;
        }
        return numArray;
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<BoxGachaEntity>(obj);
}

