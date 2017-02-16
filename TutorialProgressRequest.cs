using System;

public class TutorialProgressRequest : RequestBase
{
    public void beginRequest(int saveKey)
    {
        base.addActionField("tutorialprogress");
        base.addField("tutorialprogress", saveKey);
        base.beginRequest();
    }

    public override string getMockData() => 
        base.getMockData();

    public override string getURL() => 
        NetworkManager.getActionUrl(false);
}

