using System;

public class BattleSealedCommandWindowComponent : BattleWindowComponent
{
    public UILabel confLabel;

    public void setLabel(string str)
    {
        this.confLabel.text = str;
    }
}

