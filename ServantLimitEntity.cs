using System;
using UnityEngine;

public class ServantLimitEntity : DataEntityBase
{
    public int agility;
    public int atkBase;
    public int atkMax;
    public int criticalWeight;
    public int defense;
    public int deity;
    public int effectFolder;
    public int hpBase;
    public int hpMax;
    public int limitCount;
    public int luck;
    public int lvMax;
    public int magic;
    public int personality;
    public int policy;
    public int power;
    public int rarity;
    public int stepProbability;
    public string strParam;
    public int svtId;
    public int treasureDevice;
    public int weaponColor = 0xffffff;
    public int weaponGroup;
    public int weaponScale;

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.svtId, ":", this.limitCount };
        return string.Concat(objArray1);
    }

    public Color getWeaponColor()
    {
        int num = (this.weaponColor & 0xff0000) >> 0x10;
        int num2 = (this.weaponColor & 0xff00) >> 8;
        int num3 = this.weaponColor & 0xff;
        return new Color(((float) num) / 255f, ((float) num2) / 255f, ((float) num3) / 255f);
    }
}

