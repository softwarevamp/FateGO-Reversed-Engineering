using System;
using System.Runtime.InteropServices;

public class EventDropUpValInfo
{
    public int addCount;
    public FunctionEntity funcEntity;
    public int individuality;
    public bool isEquipUp;
    public int rateCount;

    public EventDropUpValInfo(FunctionEntity funcEntity)
    {
        this.funcEntity = funcEntity;
        this.individuality = 0;
    }

    public EventDropUpValInfo(FunctionEntity funcEntity, int individuality)
    {
        this.funcEntity = funcEntity;
        this.individuality = individuality;
    }

    public FuncList.TYPE GetFuncType() => 
        ((FuncList.TYPE) this.funcEntity.funcType);

    public void SetAddCount(int v, bool isEquipUp = false)
    {
        this.addCount = v;
        this.isEquipUp = isEquipUp;
    }

    public void SetRateCount(int v, bool isEquipUp = false)
    {
        this.rateCount = v;
        this.isEquipUp = isEquipUp;
    }
}

