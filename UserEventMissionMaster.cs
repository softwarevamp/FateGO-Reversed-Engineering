using System;
using System.Collections.Generic;

public class UserEventMissionMaster : DataMasterBase
{
    public UserEventMissionMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.USER_EVENT_MISSION);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new UserEventMissionEntity[1]);
        }
    }

    public int getAchiveMissionNum(int eventId) => 
        this.getMissionProgressNum(eventId, MissionProgressType.Type.ACHIEVE);

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<UserEventMissionEntity>(obj);

    public int getMissionProgressNum(int eventId, MissionProgressType.Type progress_type)
    {
        UserEventMissionEntity[] entityArray = this.getUserEventMissionList(eventId);
        int num = 0;
        if (entityArray.Length > 0)
        {
            foreach (UserEventMissionEntity entity in entityArray)
            {
                if (entity.missionProgressType == progress_type)
                {
                    num++;
                }
            }
        }
        return num;
    }

    public UserEventMissionEntity[] getUserEventMissionList(int eventId)
    {
        List<UserEventMissionEntity> list = new List<UserEventMissionEntity>();
        int count = base.list.Count;
        for (int i = 0; i < count; i++)
        {
            UserEventMissionEntity item = base.list[i] as UserEventMissionEntity;
            if ((item != null) && (eventId == item.eventId))
            {
                list.Add(item);
            }
        }
        return list.ToArray();
    }
}

