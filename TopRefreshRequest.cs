using System;

public class TopRefreshRequest : RequestBase
{
    public override string getMockData() => 
        string.Empty;

    public override string getURL() => 
        NetworkManager.getActionUrl(false);
}

