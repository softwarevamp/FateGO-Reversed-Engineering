using System;
using System.Runtime.InteropServices;

public class BankShopEntity : DataEntityBase
{
    public int applePrice;
    public string appleShopId;
    public int chargeStoneNum;
    public long closedAt;
    public int firstPayId;
    public int freeStoneNum;
    public int getChargeStoneNum;
    public int getFreeStoneNum;
    public string getNumDetail;
    public string getPriceDetail;
    public int googlePrice;
    public string googleShopId;
    public int id;
    public bool m_bfirst;
    public string name;
    public string numDetail;
    public long openedAt;
    public string priceDetail;
    public int stoneNum;

    public string GetCountText(bool bfirst = false)
    {
        if (!bfirst)
        {
            return LocalizationManager.GetUnitInfo(this.GetStoneNum());
        }
        return LocalizationManager.GetUnitInfo(this.GetFirstStoneNum());
    }

    public int GetFirstStoneNum() => 
        (this.getChargeStoneNum + this.getFreeStoneNum);

    public int GetPrice() => 
        this.googlePrice;

    public IconLabelInfo.IconKind GetPriceIcon() => 
        IconLabelInfo.IconKind.BANK;

    public IconLabelInfo.IconKind GetPriceUnitIcon() => 
        IconLabelInfo.IconKind.BANK_UNIT;

    public override string getPrimarykey() => 
        (string.Empty + this.id);

    public int GetStoneNum() => 
        (this.chargeStoneNum + this.freeStoneNum);
}

