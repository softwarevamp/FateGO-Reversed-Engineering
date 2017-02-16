using System;

public class BuffEntity : DataEntityBase
{
    public int buffGroup;
    public string detail;
    public int[] effectList;
    public int iconId;
    public int id;
    public int maxRate;
    public string name;
    public int[] tvals;
    public int type;
    public int[] vals;

    public bool checkBuffType(BuffList.TYPE buffType) => 
        (this.type == buffType);

    public int[] getEffectList()
    {
        if (this.effectList == null)
        {
            return new int[0];
        }
        return this.effectList;
    }

    public string getIconName() => 
        $"bufficon_{this.iconId:000}";

    public int[] getIndividuality() => 
        this.vals;

    public override string getPrimarykey() => 
        (string.Empty + this.id);

    public bool isBattleStart() => 
        (this.type == 0x53);

    public bool isCheckGroup() => 
        (this.buffGroup != 0);

    public bool isDeadAct() => 
        (this.type == 0x4e);

    public bool isEndAct() => 
        (this.type == 0x4c);

    public bool isEndSelfTurn() => 
        (this.type == 0x55);

    public bool isWaveStart() => 
        (this.type == 0x54);
}

