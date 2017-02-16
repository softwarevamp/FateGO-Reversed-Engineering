using System;
using System.Collections.Generic;

public class UserEquipMaster : DataMasterBase
{
    public UserEquipMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.USER_EQUIP);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new UserEquipEntity[1]);
        }
    }

    public void continueDeviceEquipLvInfo()
    {
        int count = base.list.Count;
        for (int i = 0; i < count; i++)
        {
            UserEquipEntity entity = base.list[i] as UserEquipEntity;
            if (entity != null)
            {
                UserEquipNewManager.SetOld(entity.equipId, entity.lv);
            }
        }
    }

    public UserEquipEntity[] getList(long userId)
    {
        int count = base.list.Count;
        List<UserEquipEntity> list = new List<UserEquipEntity>();
        for (int i = 0; i < count; i++)
        {
            UserEquipEntity item = base.list[i] as UserEquipEntity;
            if ((item != null) && (item.userId == userId))
            {
                list.Add(item);
            }
        }
        return list.ToArray();
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<UserEquipEntity>(obj);
}

