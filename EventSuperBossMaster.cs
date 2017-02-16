using System;
using System.Collections.Generic;
using UnityEngine;

public class EventSuperBossMaster : DataMasterBase
{
    protected readonly string saveKey = "EVENT_SUPERBOSS_ENCOUNTER_{0}_{1}";

    public EventSuperBossMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.EVENT_SUPER_BOSS);
        if (DataMasterBase._never)
        {
            Debug.Log(new EventSuperBossEntity[1].ToString());
        }
    }

    public List<EventSuperBossEntity> GetCurrentListByEventId(int eventId)
    {
        int num4;
        List<EventSuperBossEntity> list = new List<EventSuperBossEntity>();
        int count = base.list.Count;
        for (int i = 0; i < count; i++)
        {
            EventSuperBossEntity item = base.list[i] as EventSuperBossEntity;
            if ((item.eventId == eventId) && item.IsEncounted())
            {
                list.Add(item);
            }
        }
        if (list.Count == 0)
        {
            return null;
        }
        List<EventSuperBossEntity> list2 = new List<EventSuperBossEntity>();
        int num3 = 0;
    Label_006B:
        num4 = PlayerPrefs.GetInt(string.Format(this.saveKey, eventId, num3++), 0);
        if (num4 != 0)
        {
            for (int j = 0; j < list.Count; j++)
            {
                if (list[j].id == num4)
                {
                    list2.Add(list[j]);
                    list.RemoveAt(j);
                    break;
                }
            }
            goto Label_006B;
        }
        num3--;
        while (list.Count > 0)
        {
            list2.Add(list[0]);
            PlayerPrefs.SetInt(string.Format(this.saveKey, eventId, num3++), list[0].id);
            list.RemoveAt(0);
        }
        PlayerPrefs.Save();
        return list2;
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<EventSuperBossEntity>(obj);
}

