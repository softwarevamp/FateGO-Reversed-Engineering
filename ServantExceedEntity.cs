using System;

public class ServantExceedEntity : DataEntityBase
{
    public int addLvMax;
    public int exceedCount;
    private readonly string[] frameCardFileList = new string[] { "class_n_", "class_b_", "class_s_", "class_g_" };
    public int frameType;
    public int qp;
    public int rarity;

    public string getFrameCardPrefix() => 
        this.frameCardFileList[this.frameType];

    public override string getPrimarykey() => 
        (this.rarity.ToString() + ":" + this.exceedCount.ToString());

    public enum FRAMETYPE
    {
        BLACK,
        BRONZE,
        SILVER,
        GOLD
    }
}

