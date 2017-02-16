using System;
using UnityEngine;

public class EventRaidEntity : DataEntityBase
{
    public int bossColor;
    public int day;
    public long defeatNormaAt;
    public long endedAt;
    public int eventId;
    public int giftId;
    public int groupIndex;
    public int iconId;
    public int loginMessageId;
    public long maxHp;
    public int presentMessageId;
    public long startedAt;

    public Color GetBossColor()
    {
        int num = (this.bossColor & 0xff0000) >> 0x10;
        int num2 = (this.bossColor & 0xff00) >> 8;
        int num3 = this.bossColor & 0xff;
        return new Color(((float) num) / 255f, ((float) num2) / 255f, ((float) num3) / 255f);
    }

    public override string getPrimarykey() => 
        (this.eventId.ToString() + ":" + this.day.ToString());
}

