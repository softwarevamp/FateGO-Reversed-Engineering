using System;

public class UserExpMaster : DataMasterBase
{
    public UserExpMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.USER_EXP);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new UserExpEntity[1]);
        }
    }

    public UserExpEntity getEntityFromLevel(int lv) => 
        base.getEntityFromId<UserExpEntity>(lv);

    public int getLevel(int exp, int start_lv)
    {
        int num = this.getLevelMax();
        int lv = start_lv;
        for (int i = start_lv; i <= num; i++)
        {
            UserExpEntity entity = base.getEntityFromId<UserExpEntity>(i);
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

    public int getLevelMax() => 
        ConstantMaster.getValue("MAX_USER_LV");

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<UserExpEntity>(obj);
}

