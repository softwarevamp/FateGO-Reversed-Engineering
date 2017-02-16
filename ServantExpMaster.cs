using System;

public class ServantExpMaster : DataMasterBase
{
    public ServantExpMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.SERVANT_EXP);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new ServantExpEntity[1]);
        }
    }

    public int getLevel(int exp, int type, int max_lv, int start_lv)
    {
        int num = max_lv;
        int lv = start_lv;
        for (int i = start_lv; i <= num; i++)
        {
            ServantExpEntity entity = base.getEntityFromId<ServantExpEntity>(type, i);
            if (entity == null)
            {
                return lv;
            }
            if (exp < entity.exp)
            {
                return entity.lv;
            }
            lv = entity.lv;
        }
        return lv;
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<ServantExpEntity>(obj);
}

