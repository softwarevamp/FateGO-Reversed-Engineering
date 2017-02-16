using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class SetItemMaster : DataMasterBase
{
    public SetItemMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.SET_ITEM);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new SetItemEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<SetItemEntity>(obj);

    public SetItemEntity[] GetList(int id)
    {
        List<SetItemEntity> list = new List<SetItemEntity>();
        int count = base.list.Count;
        for (int i = 0; i < count; i++)
        {
            SetItemEntity item = base.list[i] as SetItemEntity;
            if ((item != null) && (id == item.id))
            {
                list.Add(item);
            }
        }
        return list.ToArray();
    }

    public void GetSum(out int itemNum, out int servantNum, out int servantEquipNum, int id)
    {
        ServantMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantMaster>(DataNameKind.Kind.SERVANT);
        int count = base.list.Count;
        itemNum = 0;
        servantNum = 0;
        servantEquipNum = 0;
        for (int i = 0; i < count; i++)
        {
            SetItemEntity entity = base.list[i] as SetItemEntity;
            if ((entity != null) && (id == entity.id))
            {
                if (entity.purchaseType == 1)
                {
                    itemNum += entity.setNum;
                }
                else if (entity.purchaseType == 4)
                {
                    if (master.getEntityFromId<ServantEntity>(entity.targetId).IsServantEquip)
                    {
                        servantEquipNum = entity.setNum;
                    }
                    else
                    {
                        servantNum = entity.setNum;
                    }
                }
            }
        }
    }
}

