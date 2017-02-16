using System;

public class GachaDrawRequest : RequestBase
{
    private int gachaId;
    private int num;
    private string resMsg = "MockGachaResponse";
    private int ticketItemId;

    public void beginRequest(int gachaId, int num, int warId, int ticketItemId, int shopIdIdx)
    {
        this.gachaId = gachaId;
        this.num = num;
        this.ticketItemId = ticketItemId;
        base.addActionField("gachadraw");
        base.addField("gachaId", gachaId);
        base.addField("num", num);
        base.addField("ticketItemId", ticketItemId);
        base.addField("shopIdIndex", shopIdIdx);
        base.beginRequest();
    }

    public override string getMockData() => 
        NetworkManager.getMockFile(this.resMsg);

    public override string getURL() => 
        NetworkManager.getActionUrl(false);

    public override void requestCompleted(ResponseData[] responseList)
    {
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.GACHA_DRAW, responseList);
        if (((data != null) && data.checkError()) && (data.success != null))
        {
            Debug.Log("-*-* GachaDrawRequest : " + data.success);
            base.completed(JsonManager.toJson(data.success));
            GachaEntity entity = SingletonMonoBehaviour<DataManager>.getInstance().getMasterData<GachaMaster>(DataNameKind.Kind.GACHA).getEntityFromId<GachaEntity>(this.gachaId);
            if ((this.ticketItemId != entity.ticketItemId) && (entity.type == 1))
            {
                int num = (this.num != 1) ? entity.getPayMultiTimePrice() : entity.getPayOneTimePrice();
                string str = $"gacha_id{this.gachaId}_{this.num}";
            }
        }
        else
        {
            base.completed("ng");
        }
    }

    public void setResTime(int time)
    {
        if (time > 1)
        {
            this.resMsg = "MockMultiGachaResponse";
        }
    }
}

