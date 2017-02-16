using System;

public class VoiceEntity : DataEntityBase
{
    public int condType;
    public int condValue;
    public string id;
    public string name;
    public string nameDefault;
    public int priority;
    public int svtVoiceType;

    public override string getPrimarykey() => 
        (string.Empty + this.id);
}

