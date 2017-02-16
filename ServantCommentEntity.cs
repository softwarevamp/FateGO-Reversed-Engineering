using System;
using System.Runtime.InteropServices;

public class ServantCommentEntity : DataEntityBase
{
    public string comment;
    public int condType;
    public int condValue;
    public int id;
    public int svtId;

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.svtId, ":", this.id };
        return string.Concat(objArray1);
    }

    public bool IsConst() => 
        CondType.IsConst((CondType.Kind) this.condType);

    public bool IsNew()
    {
        if (CondType.IsConst((CondType.Kind) this.condType))
        {
            return false;
        }
        if (!CondType.IsOpen((CondType.Kind) this.condType, this.condValue, NetworkManager.UserId, this.svtId))
        {
            return false;
        }
        if (ServantCommentManager.IsOpen(this.svtId, this.id))
        {
            return false;
        }
        return true;
    }

    public bool IsOpen(int oldValue = -1)
    {
        if (oldValue >= 0)
        {
            return (oldValue >= this.condValue);
        }
        return CondType.IsOpen((CondType.Kind) this.condType, this.condValue, NetworkManager.UserId, this.svtId);
    }

    public void SetOpen()
    {
        ServantCommentManager.IsOpen(this.svtId, this.id);
    }
}

