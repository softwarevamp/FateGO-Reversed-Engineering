using System;
using System.Collections.Generic;

public class NewsMaster : DataMasterBase
{
    public NewsMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.NEWS);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new NewsEntity[1]);
        }
    }

    public NewsEntity[] GetEnableEntitiyList()
    {
        Debug.Log("NewsEntity:GetEnableEntitiyList()");
        long num = NetworkManager.getTime();
        Debug.Log("nowTime[" + num + "]");
        List<NewsEntity> list = new List<NewsEntity>();
        int count = base.list.Count;
        Debug.Log("sum[" + count + "]");
        for (int i = 0; i < count; i++)
        {
            NewsEntity item = base.list[i] as NewsEntity;
            if (((item != null) && (num >= item.noticeAt)) && (num <= item.finishedAt))
            {
                list.Add(item);
            }
        }
        Debug.Log("length[" + list.Count + "]");
        return list.ToArray();
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<NewsEntity>(obj);
}

