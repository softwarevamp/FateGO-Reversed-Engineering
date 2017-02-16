using System;

public class DeckEditNameRequest : RequestBase
{
    public void beginRequest(long deckId, string deckName)
    {
        Debug.Log(string.Concat(new object[] { "DeckEditNameRequest ", deckId, " [", deckName, "]" }));
        base.addActionField("deckeditName");
        base.addField("deckId", deckId);
        base.addField("deckName", deckName);
        base.beginRequest();
    }

    public override string getMockData() => 
        string.Empty;

    public override string getURL() => 
        NetworkManager.getActionUrl(false);

    public override void requestCompleted(ResponseData[] responseList)
    {
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.DECK_EDIT_NAME, responseList);
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

