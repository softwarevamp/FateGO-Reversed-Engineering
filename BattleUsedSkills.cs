using System;

public class BattleUsedSkills
{
    public FuncInfo[] funcList;
    public int[] ptTargets;
    public int skillid;
    public int uniqueId;

    public BattleUsedSkills(int uid, int sid, int[] pts)
    {
        this.uniqueId = uid;
        this.skillid = sid;
        this.ptTargets = new int[pts.Length];
        Array.Copy(pts, 0, this.ptTargets, 0, pts.Length);
    }
}

