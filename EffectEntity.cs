using System;

public class EffectEntity : DataEntityBase
{
    public string comment;
    public int delayTime;
    public int folderType;
    public int id;
    public string name;
    public string nodeName;
    public int num;
    public string se;

    public string getNodeName()
    {
        if (this.nodeName != null)
        {
            return this.nodeName;
        }
        return null;
    }

    public override string getPrimarykey() => 
        (string.Empty + this.id);

    public bool isSe() => 
        ((this.se != null) && (0 < this.se.Length));
}

