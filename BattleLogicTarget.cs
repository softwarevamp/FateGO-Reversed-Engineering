using System;
using System.Collections.Generic;

public class BattleLogicTarget
{
    public BattleData data;

    public int getGlobalTargetId() => 
        this.data.globaltargetId;

    public int getRevenge(int actId, int[] targetlist)
    {
        int num = this.data.getServantData(actId).getRevengeTargetUniqueId();
        if (0 < num)
        {
            return num;
        }
        return this.getTargetRandom(targetlist);
    }

    public int getTargetAiAct(AiAct.TARGET target, int actId, int[] individuality, int[] targetlist)
    {
        int[] numArray = new int[0];
        numArray = this.narrowDownHate(targetlist);
        if (((numArray.Length <= 0) && (individuality != null)) && (0 < individuality.Length))
        {
            numArray = this.narrowDownIndividuality(targetlist, individuality);
        }
        if (numArray.Length <= 0)
        {
            numArray = targetlist;
        }
        if (target == AiAct.TARGET.RANDOM)
        {
            return this.getTargetRandom(numArray);
        }
        if (target == AiAct.TARGET.HP_HIGHER)
        {
            return this.getTargetHighHp(numArray);
        }
        if (target == AiAct.TARGET.HP_LOWER)
        {
            return this.getTargetLowHp(numArray);
        }
        if (target == AiAct.TARGET.NPTURN_LOWER)
        {
            return this.getTargetNpTurnLower(numArray);
        }
        if (target == AiAct.TARGET.NPGAUGE_HIGHER)
        {
            return this.getTargetNpGaugeHeighter(numArray);
        }
        if (target == AiAct.TARGET.REVENGE)
        {
            return this.getRevenge(actId, numArray);
        }
        return this.getTargetBase(numArray);
    }

    public int getTargetBase(int[] targetlist)
    {
        for (int i = 0; i < targetlist.Length; i++)
        {
            int uniqueId = targetlist[i];
            BattleServantData data = this.data.getServantData(uniqueId);
            if ((data != null) && data.isAlive())
            {
                return data.getUniqueID();
            }
        }
        return -1;
    }

    public BattleServantData getTargetBattleServantData(BattleLogicTask task)
    {
        BattleServantData data = null;
        data = this.data.getServantData(this.data.globaltargetId);
        if ((data != null) && data.isAlive())
        {
            task.setTarget(this.data.globaltargetId);
            return data;
        }
        if (((data != null) && !data.isAlive()) && data.checkOverKill(task.getActorId()))
        {
            task.setTarget(this.data.globaltargetId);
            return data;
        }
        if (((data != null) && !data.isAlive()) && data.isGuts())
        {
            task.setTarget(this.data.globaltargetId);
            if (data != null)
            {
                data.overkillTargetId = -1;
            }
            return data;
        }
        if (data != null)
        {
            data.overkillTargetId = -1;
        }
        this.data.globaltargetId = this.getTargetBase(this.data.getFieldEnemyServantIDList());
        task.setTarget(this.data.globaltargetId);
        return this.data.getServantData(this.data.globaltargetId);
    }

    public int getTargetHighHp(int[] targetlist)
    {
        Array.Sort<int>(targetlist, (x, y) => this.data.getServantData(y).hp - this.data.getServantData(x).hp);
        return this.getTargetBase(targetlist);
    }

    public int getTargetLowHp(int[] targetlist)
    {
        Array.Sort<int>(targetlist, delegate (int x, int y) {
            BattleServantData data = this.data.getServantData(x);
            BattleServantData data2 = this.data.getServantData(y);
            return data.hp - data2.hp;
        });
        return this.getTargetBase(targetlist);
    }

    public int getTargetNpGaugeHeighter(int[] targetlist)
    {
        Array.Sort<int>(targetlist, (x, y) => this.data.getServantData(y).getNp() - this.data.getServantData(x).getNp());
        return this.getTargetBase(targetlist);
    }

    public int getTargetNpTurnLower(int[] targetlist)
    {
        Array.Sort<int>(targetlist, delegate (int x, int y) {
            BattleServantData data = this.data.getServantData(x);
            BattleServantData data2 = this.data.getServantData(y);
            int num = data.getNextTDTurn() + ((0 >= data.getMaxNextTDTurn()) ? 0x3e7 : 0);
            int num2 = data2.getNextTDTurn() + ((0 >= data2.getMaxNextTDTurn()) ? 0x3e7 : 0);
            return num - num2;
        });
        return this.getTargetBase(targetlist);
    }

    public int getTargetRandom(int[] targetlist)
    {
        int[] numArray = BattleRandom.getShuffle<int>(targetlist);
        return this.getTargetBase(numArray);
    }

    private int[] narrowDownHate(int[] list)
    {
        List<int> list2 = new List<int>();
        for (int i = 0; i < list.Length; i++)
        {
            if (this.data.getServantData(list[i]).isUpHate())
            {
                list2.Add(list[i]);
            }
        }
        return list2.ToArray();
    }

    private int[] narrowDownIndividuality(int[] list, int[] targetindividuality)
    {
        List<int> list2 = new List<int>();
        for (int i = 0; i < list.Length; i++)
        {
            BattleServantData data = this.data.getServantData(list[i]);
            if (data.isAlive() && Individuality.CheckIndividualities(data.getIndividualities(), targetindividuality))
            {
                list2.Add(list[i]);
            }
        }
        return list2.ToArray();
    }

    public void setInit(BattleData data)
    {
        this.data = data;
    }
}

