using System;

public class PlayMakerUtils_FsmEvent : Attribute
{
    private string _value;

    public PlayMakerUtils_FsmEvent(string value)
    {
        this._value = value;
    }

    public string Value =>
        this._value;
}

