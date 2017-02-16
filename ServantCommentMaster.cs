using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class ServantCommentMaster : DataMasterBase
{
    public ServantCommentMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.SERVANT_COMMENT);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new ServantCommentEntity[1]);
        }
    }

    public ServantCommentEntity[] GetEntitiyList(int svtId, CondType.Kind condType = 0)
    {
        List<ServantCommentEntity> list = new List<ServantCommentEntity>();
        int num = 1;
        while (true)
        {
            object[] objArray1 = new object[] { string.Empty, svtId, ":", num };
            string key = string.Concat(objArray1);
            if (!base.lookup.ContainsKey(key))
            {
                return list.ToArray();
            }
            ServantCommentEntity item = base.lookup[key] as ServantCommentEntity;
            if ((condType == CondType.Kind.NONE) || (condType == item.condType))
            {
                list.Add(item);
            }
            num++;
        }
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<ServantCommentEntity>(obj);

    public int[] GetNewList(int svtId)
    {
        List<int> list = new List<int>();
        int num = 1;
        while (true)
        {
            object[] objArray1 = new object[] { string.Empty, svtId, ":", num };
            string key = string.Concat(objArray1);
            if (!base.lookup.ContainsKey(key))
            {
                return list.ToArray();
            }
            ServantCommentEntity entity = base.lookup[key] as ServantCommentEntity;
            if (entity.IsNew())
            {
                list.Add(entity.id);
            }
            num++;
        }
    }

    public ServantCommentEntity[] GetOpenEntitiyList(int svtId)
    {
        List<ServantCommentEntity> list = new List<ServantCommentEntity>();
        int num = 1;
        while (true)
        {
            object[] objArray1 = new object[] { string.Empty, svtId, ":", num };
            string key = string.Concat(objArray1);
            if (!base.lookup.ContainsKey(key))
            {
                return list.ToArray();
            }
            ServantCommentEntity item = base.lookup[key] as ServantCommentEntity;
            if (item.IsOpen(-1))
            {
                list.Add(item);
            }
            num++;
        }
    }

    public bool IsNew(int svtId)
    {
        int num = 1;
        while (true)
        {
            object[] objArray1 = new object[] { string.Empty, svtId, ":", num };
            string key = string.Concat(objArray1);
            if (!base.lookup.ContainsKey(key))
            {
                return false;
            }
            ServantCommentEntity entity = base.lookup[key] as ServantCommentEntity;
            if (entity.IsNew())
            {
                return true;
            }
            num++;
        }
    }

    public void SetOpen(int svtId, int svtCommentId)
    {
        ServantCommentManager.IsOpen(svtId, svtCommentId);
    }

    public void SetOpen(int svtId, int[] svtCommentIdList)
    {
        ServantCommentManager.SetOpen(svtId, svtCommentIdList);
    }
}

