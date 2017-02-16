using System;
using System.Collections.Generic;

public class BattleDeckServantData : DeckServantData
{
    public DropInfo[] dropInfos;
    public Dictionary<string, object> enemyScript;
    public int index;
    public string name;
    public int roleType;
    public int uniqueId;

    public bool checkScript(string key, int val)
    {
        if ((this.enemyScript != null) && this.enemyScript.ContainsKey(key))
        {
            int num = (int) ((long) this.enemyScript[key]);
            return (val == num);
        }
        return false;
    }

    public int getRoleType() => 
        this.roleType;

    public int getUniqueID() => 
        this.uniqueId;

    public long getUserServantID() => 
        base.userSvtId;

    public bool isAppear() => 
        this.checkScript("appear", 1);

    public bool isEscape() => 
        this.checkScript("kill", 1);
}

