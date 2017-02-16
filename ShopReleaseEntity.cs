using System;

public class ShopReleaseEntity : DataEntityBase
{
    public string closedMessage;
    public int condNum;
    public int condType;
    public int condValue;
    public bool isClosedDisp;
    public int shopId;

    public string GetPreparationConditionText()
    {
        if (string.IsNullOrEmpty(this.closedMessage))
        {
            return CondType.OpenConditionText((CondType.Kind) this.condType, this.condValue);
        }
        return this.closedMessage;
    }

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.shopId, ":", this.condType };
        return string.Concat(objArray1);
    }

    protected bool IsCondEnable() => 
        CondType.IsOpen((CondType.Kind) this.condType, this.condValue, 0);

    public bool IsOpen()
    {
        if (!this.IsClosedDisp)
        {
            return this.IsCondEnable();
        }
        return true;
    }

    public bool IsPreparation() => 
        (this.IsClosedDisp && !this.IsCondEnable());

    public bool IsClosedDisp =>
        this.isClosedDisp;
}

