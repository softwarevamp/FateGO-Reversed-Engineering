using System;

public class DeckSetupRequest : RequestBase
{
    public void beginRequest(long mainDeckId, long activeDeckId, UserDeckEntity userDeck)
    {
        string str = JsonManager.toJson(userDeck);
        Debug.Log(string.Concat(new object[] { "DeckSetupRequest ", mainDeckId, " [", str, "]" }));
        base.addActionField("decksetup");
        base.addField("mainDeckId", mainDeckId);
        base.addField("activeDeckId", activeDeckId);
        base.addField("userDeck", "[" + str + "]");
        base.beginRequest();
    }

    public override string getMockData() => 
        string.Empty;

    public override string getURL() => 
        NetworkManager.getActionUrl(false);

    public override void requestCompleted(ResponseData[] responseList)
    {
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.DECK_SETUP, responseList);
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

