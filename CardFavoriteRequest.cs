using System;

public class CardFavoriteRequest : RequestBase
{
    public void beginRequest(long targetUsrSVtId)
    {
        base.addActionField("cardfavorite");
        base.addField("userSvtId", targetUsrSVtId);
        base.beginRequest();
    }

    public void beginRequest(long targetUsrSVtId, int imageLimitCount, int dispLimitCount, int commandCardLimitCount, int iconLimitCount, bool isFavorite)
    {
        base.addActionField("cardfavorite");
        base.addField("userSvtId", targetUsrSVtId);
        base.addField("imageLimitCount", imageLimitCount);
        base.addField("dispLimitCount", dispLimitCount);
        base.addField("isFavorite", !isFavorite ? 0 : 1);
        base.addField("commandCardLimitCount", commandCardLimitCount);
        base.addField("iconLimitCount", iconLimitCount);
        base.beginRequest();
    }

    public override string getMockData() => 
        NetworkManager.getMockFile(string.Empty);

    public override string getURL() => 
        NetworkManager.getActionUrl(false);

    public override void requestCompleted(ResponseData[] responseList)
    {
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.SET_FAVORITE_SERVANT, responseList);
        if ((data != null) && data.checkError())
        {
            base.completed("ok");
        }
        else
        {
            base.completed("ng");
        }
    }
}

