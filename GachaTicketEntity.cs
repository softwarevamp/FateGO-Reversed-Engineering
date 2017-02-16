using System;

public class GachaTicketEntity : DataEntityBase
{
    public int gachaId;
    public int gachaProbabilityId;
    public int gachaRarityId;
    public int ticketItemId;

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.gachaId, ":", this.ticketItemId };
        return string.Concat(objArray1);
    }
}

