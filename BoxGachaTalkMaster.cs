using System;
using System.Collections.Generic;

public class BoxGachaTalkMaster : DataMasterBase
{
    public BoxGachaTalkMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.BOX_GACHA_TALK);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new BoxGachaTalkEntity[1]);
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<BoxGachaTalkEntity>(obj);

    public BoxGachaTalkEntity[] getTalkData(int id)
    {
        List<BoxGachaTalkEntity> list = new List<BoxGachaTalkEntity>();
        int count = base.list.Count;
        for (int i = 0; i < count; i++)
        {
            BoxGachaTalkEntity item = base.list[i] as BoxGachaTalkEntity;
            if ((item != null) && ((id <= 0) || (id == item.id)))
            {
                list.Add(item);
            }
        }
        return list.ToArray();
    }
}

