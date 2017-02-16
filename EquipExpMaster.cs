using System;

public class EquipExpMaster : DataMasterBase
{
    public EquipExpMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.EQUIP_EXP);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new EquipExpEntity[1]);
        }
    }

    public int getLevel(int equipId, int exp, int start_lv)
    {
        int num = this.getLevelMax(equipId);
        int lv = start_lv;
        for (int i = start_lv; i <= num; i++)
        {
            EquipExpEntity entity = base.getEntityFromId<EquipExpEntity>(equipId, i);
            if (entity == null)
            {
                return lv;
            }
            if (exp <= entity.exp)
            {
                return entity.lv;
            }
            lv = entity.lv;
        }
        return lv;
    }

    public int getLevelMax(int id)
    {
        int count = base.list.Count;
        int lv = 0;
        for (int i = 0; i < count; i++)
        {
            EquipExpEntity entity = base.list[i] as EquipExpEntity;
            if (((entity != null) && (entity.equipId == id)) && (entity.lv > lv))
            {
                lv = entity.lv;
            }
        }
        return lv;
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<EquipExpEntity>(obj);
}

