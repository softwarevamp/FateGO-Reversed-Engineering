using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

public class StageEntity : DataEntityBase
{
    [CompilerGenerated]
    private static Converter<object, int> <>f__am$cacheA;
    public int bgmId;
    public static readonly int DEFAULT_ENEMY_ACTION_COUNT = 3;
    public int enemyInfo;
    public string name;
    public long[] npcDeckIds;
    public int questId;
    public int questPhase;
    public Dictionary<string, object> script;
    public int startEffectId;
    public int wave;

    public StageEntity()
    {
    }

    public StageEntity(StageEntity cSrc)
    {
        this.questId = cSrc.questId;
        this.questPhase = cSrc.questPhase;
        this.name = cSrc.name;
        this.wave = cSrc.wave;
        this.npcDeckIds = cSrc.npcDeckIds;
    }

    public bool checkScript(string key) => 
        ((this.script != null) && this.script.ContainsKey(key));

    public int getEnemyActCount()
    {
        int num = this.getScript("EnemyActCount", 0);
        if (0 < num)
        {
            return num;
        }
        return DEFAULT_ENEMY_ACTION_COUNT;
    }

    public int getLimitAct() => 
        this.getScript("LimitAct", 0);

    public int getLimitTurn() => 
        this.getScript("turn", 0);

    public int getPhase() => 
        this.questPhase;

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.questId, ":", this.questPhase, ":", this.wave };
        return string.Concat(objArray1);
    }

    public int getQuestId() => 
        this.questId;

    public int getScript(string key, int defVal = 0)
    {
        if (this.checkScript(key))
        {
            return (int) ((long) this.script[key]);
        }
        return defVal;
    }

    public int getShowTurnState() => 
        this.getScript("showTurn", 2);

    public int[] getTurnEffect(int limitTurn)
    {
        if ((this.script == null) || !this.script.ContainsKey("turnEffect"))
        {
            return new int[0];
        }
        List<object> list = (List<object>) this.script["turnEffect"];
        if (<>f__am$cacheA == null)
        {
            <>f__am$cacheA = x => int.Parse(string.Empty + x);
        }
        int[] numArray = list.ConvertAll<int>(<>f__am$cacheA).ToArray();
        int[] numArray2 = new int[limitTurn];
        int num = 0;
        int index = 0;
        for (int i = 0; i < numArray2.Length; i++)
        {
            numArray2[i] = num;
            if ((index < numArray.Length) && (i == (numArray[index] - 1)))
            {
                num++;
                index++;
            }
        }
        return numArray2;
    }

    public int getTurnEffectType() => 
        this.getScript("turnEffectType", 0);

    public int getWave() => 
        this.wave;

    public bool isNotDisplayRemain() => 
        (this.enemyInfo == 2);
}

